using System;

namespace AbsenceApp {
    public interface ILocation {
        void ObtainMyLocation();
        event EventHandler<ILocationEventArgs> locationObtained;
    }
    public interface ILocationEventArgs {
        double lat { get; set; }
        double lng { get; set; }
    }
}