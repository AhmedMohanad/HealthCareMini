namespace HealthcareMini.Models.Enums
{
    public enum AppointmentStatus
    {

        Scheduled = 0,  // patient has booked waiting for visit day 
        CheckedIn = 1,  // patient arrived  
        Completed = 2,  // patient had his visit and left the clinic
        Canceled  = 3,  // appointment was called off before the visit
        NoShow    = 4   // patient did not show up and did not cancel .

    }
}
