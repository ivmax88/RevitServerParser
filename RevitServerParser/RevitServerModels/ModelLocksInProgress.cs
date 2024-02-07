﻿using System.Text.Json.Serialization;

namespace RevitServerParser.RevitServerModels
{
    public class ModelLocksInProgress
    {
        public string? Age { get; set; }
        public int ModelLockOptions { get; set; }
        public int ModelLockType { get; set; }
        public string? ModelPath { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string? UserName { get; set; }
    }
}
