using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mascis.Models
{
    public class EventsRepository
    {
        public static IList<ScheduleEvent> GetAllRecords()
        {
            IList<ScheduleEvent> appoint = (IList<ScheduleEvent>)HttpContext.Current.Session["Appointments"];
            if (appoint == null)
                HttpContext.Current.Session["Appointments"] = appoint = new mascisEntities().ScheduleEvents.ToList();
            return appoint;
        }

        public static List<ScheduleEvent> FilterAppointment(DateTime CurrentDate, String CurrentAction, String CurrentView)
        {
            DateTime CurrDate = Convert.ToDateTime(CurrentDate);
            DateTime StartDate = FirstWeekDate(CurrDate.Date);
            DateTime EndDate = FirstWeekDate(CurrDate.Date);
            List<ScheduleEvent> appointmentList = new mascisEntities().ScheduleEvents.Where(a => a.IsBlockAppointment != true).ToList(); // GetAllRecords().ToList();
            switch (CurrentView)
            {
                case "day":
                    StartDate = CurrentDate;
                    EndDate = CurrentDate;
                    break;
                case "week":
                    EndDate = EndDate.AddDays(7);
                    break;
                case "workweek":
                    EndDate = EndDate.AddDays(5);
                    break;
                case "month":
                    StartDate = CurrDate.Date.AddDays(-CurrDate.Day + 1);
                    EndDate = StartDate.AddMonths(1);
                    break;
                case "Agenda":
                    EndDate = EndDate.AddDays(14);
                    break;
            }
           // DateTime st; DateTime et;
            //appointmentList = new mascisEntities().ScheduleEvents.ToList().
            //    Where(app => DateTime.TryParse(app.StartTime.ToString(), out st) &&
            //    ((st.ToLocalTime().Date >= Convert.ToDateTime(StartDate.Date)) &&
            //    DateTime.TryParse(app.EndTime.ToString(), out et) &&
            //    (et.ToLocalTime().Date <= Convert.ToDateTime(EndDate.Date))) || app.Recurrence == true).Where(a => a.IsBlockAppointment != true).ToList();
            return appointmentList;
        }

        internal static DateTime FirstWeekDate(DateTime CurrentDate)
        {
            try
            {
                DateTime FirstDayOfWeek = CurrentDate;
                DayOfWeek WeekDay = FirstDayOfWeek.DayOfWeek;
                switch (WeekDay)
                {
                    case DayOfWeek.Sunday:
                        break;
                    case DayOfWeek.Monday:
                        FirstDayOfWeek = FirstDayOfWeek.AddDays(-1);
                        break;
                    case DayOfWeek.Tuesday:
                        FirstDayOfWeek = FirstDayOfWeek.AddDays(-2);
                        break;
                    case DayOfWeek.Wednesday:
                        FirstDayOfWeek = FirstDayOfWeek.AddDays(-3);
                        break;
                    case DayOfWeek.Thursday:
                        FirstDayOfWeek = FirstDayOfWeek.AddDays(-4);
                        break;
                    case DayOfWeek.Friday:
                        FirstDayOfWeek = FirstDayOfWeek.AddDays(-5);
                        break;
                    case DayOfWeek.Saturday:
                        FirstDayOfWeek = FirstDayOfWeek.AddDays(-6);
                        break;
                }
                return (FirstDayOfWeek);
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }
}