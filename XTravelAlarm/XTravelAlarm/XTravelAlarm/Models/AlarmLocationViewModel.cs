﻿﻿using System;
using System.Windows.Input;
using Prism.Mvvm;

namespace XTravelAlarm.Models
{
    public class AlarmLocationViewModel : BindableBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public double Distance { get; set; }

        private bool _isRunning;

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                if (_isRunning == value)
                {
                    return;
                }
                SetProperty(ref _isRunning, value);
                RunningStatusChanged?.Execute(value);
            }
        }

        public ICommand RunningStatusChanged { get; set; }
    }
}