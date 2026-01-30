using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class PostAnimalReq : Common
    {
        [Required(ErrorMessage = "Der skal være mindst et tilkoblet opslag")]
        public required string PostId { get; set; }
        public Post? Post { get; set; }

        [Required(ErrorMessage = "Der skal være mindst et tilkoblet dyr")]
        public required string AnimalId { get; set; }
        public Animal? Animal { get; set; }
    }
}
