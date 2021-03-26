using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using SmartCoop.Core.Coop;
using SmartCoop.Core.Devices;
using SmartCoop.Infrastructure.Coop;

namespace SmartCoop.Web.Pages
{
    public partial class Index : IDisposable
    {
        [Inject] private IMapper Mapper { get; set; }
        [Inject] public ICoop Coop { get; set; }
        [Inject] public IEnumerable<IDevice> AvailaibleDevices { get; set; }
        public List<IDevice> Devices { get; set; }
        public bool DialogOpen { get; set; }
        public string DeviceType { get; set; }

        public bool Editing { get; set; }

        protected override void OnInitialized()
        {
            Coop.PropertyChanged += CoopOnPropertyChanged;
        }

        private void CoopOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public async Task Save()
        {
            Coop.Dispose();
            Coop.Devices = Devices;
            await Coop.Save();
            await Coop.Initialize();
            Editing = false;
        }

        public void Edit()
        {
            Coop.Dispose();
            Devices = Coop.CopyDevices();
            Editing = true;
        }

        public async Task Cancel()
        {
            await Coop.Initialize();
            Editing = false;
        }

        public void Remove(IDevice device)
        {
            Devices.Remove(device);
        }

        public void OpenAddDevice()
        {
            DialogOpen = true;
        }

        public void AddDevice()
        {
            if (!string.IsNullOrEmpty(DeviceType))
            {
                var assembly = typeof(Coop).Assembly;
                Type t = assembly.GetType(DeviceType);
                if (t != null)
                {
                    var device = Activator.CreateInstance(t);
                    Devices.Add((IDevice)device);
                }
            }
            DialogOpen = false;
        }

        public void Dispose()
        {
            Coop.PropertyChanged -= CoopOnPropertyChanged;
        }

    }
}
