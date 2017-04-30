using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareemDataFeeder;

namespace CareemDataFeeder
{
    class Program
    {
        static void Main(string[] args)
        {
            //MoveDataToDatabase();
            BuildDestinationsTable();
        }

        private static void BuildDestinationsTable()
        {
            RidesModel model = new RidesModel();
            foreach(var ride in model.Rides.ToList())
            {
                UserHour hour = new UserHour();
                hour.userId = ride.userId;
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime pickupTime =  epoch.AddSeconds(ride.pickupTime);
                hour.hour = int.Parse(pickupTime.ToString("HH"));

                var existingUserHour = model.UserHours.FirstOrDefault(x => x.userId == hour.userId && x.hour == hour.hour);

                if (existingUserHour == null)
                {
                    model.UserHours.Add(hour);
                    model.SaveChanges();
                    existingUserHour = hour;
                }

                var existingDestination = model.DestinationsByHours.FirstOrDefault(x => x.userHourId == existingUserHour.Id);

                if (existingDestination == null)
                {
                    DestinationsByHour newDestHour = new DestinationsByHour() { dropoffLocation = ride.dropoffDisplay, dropoffLat = ride.dropoffLat, dropoffLong = ride.dropoffLong, count = 1, userHourId = existingUserHour.Id};
                    model.DestinationsByHours.Add(newDestHour);
                    model.SaveChanges();
                }
                else
                {
                    existingDestination.count = existingDestination.count + 1;

                    model.Entry(existingDestination).State = System.Data.Entity.EntityState.Modified;     

                    model.SaveChanges();
                }
            }
        }

        private static void MoveDataToDatabase()
        {
            RidesModel model = new RidesModel();



            int index = 0;
            using (TextFieldParser parser = new TextFieldParser("large.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    //Process row
                    string[] fields = parser.ReadFields();

                    if (index <= 2)
                    {
                        index += 1;
                        continue;
                    }


                    Ride ride = new Ride();

                    ride.userId = fields[0];
                    ride.rideId = fields[1];
                    ride.pickupTime = long.Parse(fields[2], System.Globalization.CultureInfo.InvariantCulture);
                    ride.pickupDisplay = fields[3];
                    ride.pickupLat = double.Parse(fields[4]);
                    ride.pickupLong = double.Parse(fields[5]);
                    ride.pickupGeohash = fields[6];
                    ride.dropoffDisplay = fields[7];
                    ride.dropoffLat = double.Parse(fields[8]);
                    ride.dropoffLong = double.Parse(fields[9]);
                    ride.dropoffGeohash = fields[10];

                    model.Rides.Add(ride);

                }

                model.SaveChanges();
                model.Dispose();
            }
        }
    }
}
