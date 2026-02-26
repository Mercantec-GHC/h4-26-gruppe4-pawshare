using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOs
{
    public class AnimalDto : Common
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string AnimalPictureKey { get; set; }
        public required DateOnly DateOfBirth { get; set; }

        public required string UserName { get; set; }
        public required string UserId { get; set; }

        public required string TypeId { get; set; }
        public required string TypeName { get; set; }
        public required string TypeDescription { get; set; }

        public required List<AnimalBookingDto> Bookings { get; set; }
    }

    public class CreateAnimalTypeDto
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }

    public class UpdateAnimalPictureDto
    {
        public required string NewAnimalPictureKey { get; set; }
        public string? OldAnimalPictureKey { get; set; }
    }
}
