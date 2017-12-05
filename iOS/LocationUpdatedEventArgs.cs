using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using CoreLocation;

namespace AbsenceApp.iOS
{
    public class LocationUpdatedEventArgs
    {
        CLLocation location;

        public LocationUpdatedEventArgs(CLLocation location)
        {
            this.location = location;
        }

        public CLLocation Location {
            get { return location; }
        }
    }
}