﻿namespace Dto;

public class ConfirmEmailDto
{
    public string Email { get; set; }
    public string EmailConfirmationToken { get; set; }
}