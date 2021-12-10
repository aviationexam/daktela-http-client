using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets
{
    public class Ticket
    {
        /// <summary>
        /// Unique name
        /// </summary>
        [JsonPropertyName("name")]
        public int Name { get; private set; }

        /// <summary>
        /// Subject of ticket
        /// </summary>
        [JsonPropertyName("title")]
        [Required]
        public string Title { get; set; } = null!;

        /// <summary>
        /// A merged ticket Id
        /// </summary>
        [JsonPropertyName("id_merge")]
        public int MergeId { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("category")]
        [Required]
        public string Category { get; set; } = null!;

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("user")]
        public int User { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("contact")]
        public int Contact { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("parentTicket")]
        public int ParentTicketId { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("isParent")]
        public bool IsParent { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("stage")]
        public EStage Stage { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("priority")]
        public EPriority Priority { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("sla_overdue")]
        public int SlaOverdue { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("sla_deadtime")]
        public int SlaDeadTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("sla_close_deadline")]
        public int SlaCloseDeadline { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("sla_change")]
        public int SlaChange { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("sla_duration")]
        public int SlaDuration { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("sla_custom")]
        public int SlaCustom { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("interaction_activity_count")]
        public int InteractionActivityCount { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("reopen")]
        public int Reopen { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("created")]
        public int Created { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("created_by")]
        public int CreatedBy { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("edited")]
        public int Edited { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("edited_by")]
        public int EditedBy { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("first_answer")]
        public int FirstAnswer { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("first_answer_duration")]
        public int FirstAnswerDuration { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("first_answer_deadline")]
        public int FirstAnswerDeadline { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("first_answer_overdue")]
        public int FirstAnswerOverdue { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("closed")]
        public int Closed { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("unread")]
        public int Unread { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("has_attachment")]
        public int HasAttachment { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("followers")]
        public int Followers { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("statuses")]
        public int Statuses { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("last_activity")]
        public int LastActivity { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("last_activity_operator")]
        public int LastActivityOperator { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("last_activity_client")]
        public int LastActivityClient { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("customFields")]
        public int CustomFields { get; set; }
    }
}
