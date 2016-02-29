﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SC.BL.Domain
{
    [Serializable]
    public class Ticket
    {
        //[Key] /* TOEGEVOEGD, nadien VERWIJDERD 'Fluent API' */
        public int TicketNumber { get; set; }
        public int AccountId { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Er zijn maximaal 100 tekens toegestaan")]
        public string Text { get; set; }

        public DateTime DateOpened { get; set; }
        //[Index] /* TOEGEVOEGD, nadien VERWIJDERD 'Fluent API' */
        public TicketState State { get; set; }

        public virtual ICollection<TicketResponse> Responses { get; set; }
        /* TOEGEVOEGD 'virtual' for lazy-loading, if enabled on context (default) */
    }
}