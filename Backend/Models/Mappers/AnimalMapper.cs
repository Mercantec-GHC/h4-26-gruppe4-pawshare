using Models;
using Models.DTOs;

public static class AnimalMapper
{
    public static AnimalDto ToDto(Animal a)
    {
        return new AnimalDto
        {
            Id = a.Id,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt,

            Name = a.Name,
            Description = a.Description,
            Base64Image = a.Base64Image,
            DateOfBirth = a.DateOfBirth,

            UserId = a.UserId,
            UserName = a.User?.Name,

            TypeId = a.TypeId,
            TypeName = a.AnimalType?.Name,
            TypeDescription = a.AnimalType?.Description,

            Bookings = a.Bookings?.Select(b => new AnimalBookingDto
            {
                AppointmentId = b.AppointmentId,
                Start = b.Appointment?.Start,
                End = b.Appointment?.End,
                Address = b.Appointment?.Address
            }).ToList() ?? new List<AnimalBookingDto>()
        };
    }

    public static void MapToEntity(AnimalDto dto, Animal entity)
    {
        entity.Id = dto.Id;
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Base64Image = dto.Base64Image;
        entity.DateOfBirth = dto.DateOfBirth;
        entity.TypeId = dto.TypeId;
        entity.UserId = dto.UserId;
    }
}
