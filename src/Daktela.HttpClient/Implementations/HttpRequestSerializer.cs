using Daktela.HttpClient.Interfaces;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Implementations;

public class HttpRequestSerializer : IHttpRequestSerializer
{
    public HttpContent SerializeRequest<TContract>(
        TContract contract,
        JsonTypeInfo<TContract> jsonTypeInfoForRequestType
    ) where TContract : class => JsonContent<TContract>.Create(
        contract,
        jsonTypeInfoForRequestType
    );

    private sealed class JsonContent<TValue> : HttpContent
    {
        private readonly JsonTypeInfo<TValue> _jsonTypeInfoForRequestType;

        private TValue Value { get; }

        private JsonContent(
            TValue inputValue,
            MediaTypeHeaderValue? mediaType,
            JsonTypeInfo<TValue> jsonTypeInfoForRequestType
        )
        {
            Value = inputValue;
            Headers.ContentType = mediaType ?? JsonHelpers.GetDefaultMediaType();
            _jsonTypeInfoForRequestType = jsonTypeInfoForRequestType;
        }

        public static JsonContent<T> Create<T>(
            T inputValue,
            JsonTypeInfo<T> jsonTypeInfoForRequestType,
            MediaTypeHeaderValue? mediaType = null
        ) => new(
            inputValue, mediaType, jsonTypeInfoForRequestType
        );

        protected override Task SerializeToStreamAsync(
            Stream stream, TransportContext? context
        ) => SerializeToStreamAsyncCore(stream, CancellationToken.None);

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }

        private async Task SerializeToStreamAsyncCore(
            Stream targetStream,
            CancellationToken cancellationToken
        )
        {
            var targetEncoding = JsonHelpers.GetEncoding(Headers.ContentType?.CharSet);

            // Wrap provided stream into a transcoding stream that buffers the data transcoded from utf-8 to the targetEncoding.
            if (targetEncoding != null && !targetEncoding.Equals(Encoding.UTF8))
            {
                await using var transcodingStream = Encoding.CreateTranscodingStream(
                    targetStream, targetEncoding, Encoding.UTF8, leaveOpen: true
                );

                await JsonSerializer.SerializeAsync(
                    transcodingStream, Value, _jsonTypeInfoForRequestType,
                    cancellationToken
                ).ConfigureAwait(false);
            }
            else
            {
                await JsonSerializer.SerializeAsync(
                    targetStream, Value, _jsonTypeInfoForRequestType,
                    cancellationToken
                ).ConfigureAwait(false);
            }
        }
    }

    private static class JsonHelpers
    {
        internal static MediaTypeHeaderValue GetDefaultMediaType() => new("application/json")
        {
            CharSet = "utf-8",
        };

        internal static Encoding? GetEncoding(string? charset)
        {
            Encoding? encoding = null;

            if (charset != null)
            {
                try
                {
                    // Remove at most a single set of quotes.
                    if (charset.Length > 2 && charset[0] == '\"' && charset[^1] == '\"')
                    {
                        encoding = Encoding.GetEncoding(charset.Substring(1, charset.Length - 2));
                    }
                    else
                    {
                        encoding = Encoding.GetEncoding(charset);
                    }
                }
                catch (ArgumentException e)
                {
                    throw new InvalidOperationException("CharSetInvalid", e);
                }
            }

            return encoding;
        }
    }
}
