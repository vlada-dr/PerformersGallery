using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PerformersGallery.Services
{
    public class CheckUpdatesService
    {
        private readonly FlickrService _flickrService;
        public CheckUpdatesService(FlickrService flickrService)
        {
            _flickrService = flickrService;
            Thread newThread = new Thread(new ThreadStart(AutoRefresh));
            newThread.Start();
        }

        private async void AutoRefresh()
        {
            do
            {
                Random rand = new Random();
                await _flickrService.RefreshPhotos();
                if(DateTime.Now.Hour > 22)
                {
                    Thread.Sleep(21600000);
                }
                Thread.Sleep(rand.Next(10000, 300000));
            }
            while (true);
        }

    }
}
