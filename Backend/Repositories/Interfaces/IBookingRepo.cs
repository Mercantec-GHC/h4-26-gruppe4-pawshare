using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Interfaces
{
    public interface IBookingRepo
    {
        /// <summary>
        /// Posts a new Booking to the table
        /// </summary>
        /// <param name="newBookingType">The new Booking that needs to be created</param>
        /// <returns>Booking that was added, null if it already exists, and throws exception if error occurs under creation</returns>
        public Task<AppointmentAnimalBooking?> CreateBooking(string AppointmentId, string AnimalId);

        /// <summary>
        /// Delets Booking from table
        /// </summary>
        /// <param name="BookingId">Id of the Booking needed to be deleted</param>
        /// <returns>Boolean, true if succesful and false if not</returns>
        public Task<bool> DeleteBooking(string AppointmentId, string AnimalId);
    }
}
