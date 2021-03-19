using System;
using System.Collections;
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
        public ICoop TempCoop { get; set; }
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
            await TempCoop.Save();
            Coop.Dispose();
            Coop.Devices = TempCoop.Devices;
            Coop.Initialize();
            Editing = false;
        }

        public void Edit()
        {
            TempCoop = Mapper.Map<ICoop>(Coop);
            Editing = true;
        }

        public void Cancel()
        {
            Editing = false;
        }

        public void Remove(IDevice device)
        {
            TempCoop.Devices.Remove(device);
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
                    TempCoop.Devices.Add((IDevice)device);
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
