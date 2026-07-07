using System;

namespace Application.DTOs.TimeSlot;

public class TimeSlotConfigDto
{
    public Guid TimeSlotId { get; set; }
    public int MaxCapacity { get; set; }
}
