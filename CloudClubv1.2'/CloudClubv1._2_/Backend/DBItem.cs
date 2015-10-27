using System;

namespace Backend
{
    public interface DBItem
    {
        string Id { get; set; }

        DateTime? Time { get; set; }
    }
}