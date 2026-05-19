namespace HealthcareMini.Models.Enums
{
    public enum UserRole
    {

        // this is enum to define what user can do in the system.

        Admin = 0,        // Full access - do every thing
        Doctor = 1,       // Sees own patients, see schedule, and medical records of their patient
        Receptionist = 2, // Manages appointments; no access to medical records
        Patient = 3        // he is sick he cant do anything but see his medical records and appointments


    }
}
