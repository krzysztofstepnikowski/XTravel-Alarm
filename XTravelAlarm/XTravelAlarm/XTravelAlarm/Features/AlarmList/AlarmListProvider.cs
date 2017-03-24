﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Geolocator;
using XTravelAlarm.Features.AlarmRinging;
using XTravelAlarm.Views.Alarms;

namespace XTravelAlarm.Features.AlarmList
{
    public class AlarmListProvider : IAlarmPageFeatures
    {
        private readonly HashSet<Location> alarms;
        private AlarmCaller alarmCaller;
        private readonly IRinger ringer;

        public AlarmListProvider(HashSet<Location> alarms, IRinger ringer)
        {
            this.alarms = alarms;
            this.ringer = ringer;
        }

        public IEnumerable<Location> GetAll()
        {
            return alarms.Select(x => new Location
            {
                Name = x.Name,
                Distance = x.Distance
            }).ToList();
        }

        public async Task Add(Location alarmLocation)
        {
            alarms.Add(alarmLocation);


            alarmCaller = new AlarmCaller(alarmLocation.Position, alarmLocation.Distance, ringer);


            CrossGeolocator.Current.PositionChanged += CurrentPositionChanged;


            await CrossGeolocator.Current.StartListeningAsync(minTime: 1000,
                minDistance: alarmLocation.Distance * 1000);
        }

        private void CurrentPositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            var position = e.Position;

            alarmCaller.UpdatePosition(new Position(position.Latitude,position.Longitude));

            Debug.WriteLine($"Full: Lat: {position.Latitude}, {position.Longitude}");
            Debug.WriteLine($"Time: {position.Timestamp}");
            Debug.WriteLine($"Accurancy {position.Accuracy}");
        }
    }
}
