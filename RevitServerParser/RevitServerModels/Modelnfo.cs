﻿namespace RevitServerParser.RevitServerModels
{
    public class Modelnfo
    {
        public string? Path { get; set; }
        public string? DateCreated { get; set; }
        public string? DateModified { get; set; }
        public string? LastModifiedBy { get; set; }
        public string? ModelGUID { get; set; }
        public long ModelSize { get; set; }
        public long SupportSize { get; set; }
    }
}
