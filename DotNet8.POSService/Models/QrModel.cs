﻿namespace DotNet8.POS.PosService.Models;

public class QrModel
{
    public string Id { get; set; }

    public string Code { get; set; }

    public string Type { get; set; }

    public bool isSuccess { get; set; }

    public string Message { get; set; }
}