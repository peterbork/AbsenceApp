using System;

namespace AbsenceApp {
    public interface ILocation {
        void StartListener(double lat, double lng, int distance);
        event EventHandler<ILocationEventArgs> locationObtained;
    }
    public interface ILocationEventArgs {
        double lat { get; set; }
        double lng { get; set; }
    }
}