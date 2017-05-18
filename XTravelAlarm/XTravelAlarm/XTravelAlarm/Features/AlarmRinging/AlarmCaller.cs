﻿using System;
using XTravelAlarm.Features.AlarmRinging.Storage;

namespace XTravelAlarm.Features.AlarmRinging
{
    public class AlarmCaller
    {
        private readonly IRinger ringer;
        private readonly IAlarmStorage alarmStorage;
        private readonly INotificationService notificationService;

        public AlarmCaller(IRinger ringer, IAlarmStorage alarmStorage, INotificationService notificationService)
        {
            this.ringer = ringer;
            this.alarmStorage = alarmStorage;
            this.notificationService = notificationService;
        }


        private double CalculateDistance(Position position, Position alarmPosition)
        {
            return CalculateWithHaversine(position, alarmPosition);
        }

        private double CalculateWithHaversine(Position position, Position alarmPosition)
        {
            var R = 6371d;
            var currentLatitude = position.Latitude;
            var currentLongitude = position.Longitude;


            var alarmLatitude = alarmPosition.Latitude;
            var alarmLongitude = alarmPosition.Longitude;

            var differenceLatitudes = ToRadius(alarmLatitude - currentLatitude);
            var differenceLongitudes = ToRadius(alarmLongitude - currentLongitude);

            var a = Math.Sin(differenceLatitudes / 2d) * Math.Sin(differenceLatitudes / 2d) +
                    Math.Cos(ToRadius(currentLatitude)) * Math.Cos(ToRadius(alarmLatitude)) *
                    Math.Sin(differenceLongitudes / 2d) * Math.Sin(differenceLongitudes / 2d);

            var c = 2d * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1d - a));


            return R * c;
        }

        private double ToRadius(double deg)
        {
            return deg * (Math.PI / 180d);
        }


        public async void UpdatePosition(Position position, Guid alarmId)
        {
            var alarm = alarmStorage.GetAlarm(alarmId);


            var currentDistance = CalculateDistance(position, alarm.Destination);
            if (currentDistance <= alarm.Distance)
            {
                await ringer.PlaySoundAsync("MySong.mp3");
                notificationService.Show("Alarm", "Wyłącz alarm", alarmId);

            }
        }
    }
}
