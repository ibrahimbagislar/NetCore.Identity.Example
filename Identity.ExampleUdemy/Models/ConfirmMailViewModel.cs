﻿namespace NetCore.Identity.Example.Models
{
    public class ConfirmMailViewModel
    {
        public int Id { get; set; }
        public int ConfirmCode { get; set; }
        public string Email { get; set; }
    }
}