﻿
namespace Service.Models
{
    public class ResultContext<T>
    {
        public T? Value { get; set; }
        public ErrorContext? Error { get; set; }
    }
}
