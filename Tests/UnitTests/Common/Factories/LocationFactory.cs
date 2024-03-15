using VIAEventAssociation.Core.Domain.Aggregates.Locations;

namespace UnitTests.Common.Factories;

public static class LocationFactory {

    public static IEnumerable<object[]> GetValidLocationNames() {
        return new List<object[]>() {
            new object[] {"A01.01"},
            new object[] {"A03.15"},
            new object[] {"A05.99"},
            new object[] {"B02.01"},
            new object[] {"B05.06"},
            new object[] {"B03.69"},
            new object[] {"C01.20"},
            new object[] {"C03.11"},
            new object[] {"C04.18"},
        }; 
    }

    public static IEnumerable<object[]> GetInValidLocationNames() {
        return new List<object[]>() {
            new object[] {""},                                 
            new object[] {"  "},
            new object[] {"HELLO"},
            new object[] {"B0201"},
            new object[] {"B05.06.07"},
            new object[] {"Z01.20"},
            new object[] {"D01.20"},
            new object[] {"D03.11"},
            new object[] {"C03.100"},
            new object[] {"C04.150"},
            new object[] {"AAA.41"},
            new object[] {"AAA.41"},
            new object[] {"A02.1"},
            new object[] {"A2.10"},
        }; 
    }

    public static IEnumerable<object[]> GetValidLocationMaxGuests() {
        return new List<object[]>() {
            new object[] {5},
            new object[] {50},
            new object[] {10},
            new object[] {20},
            new object[] {30},
            new object[] {40}
        };
    }

    public static IEnumerable<object[]> GetInvalidLocationMaxGuests() {
        return new List<object[]>() {
            new object[] {0},
            new object[] {2},
            new object[] {4},
            new object[] {51},
            new object[] {100},
            new object[] {1500},
        };
    }

    public static Location GetValidLocation() {
        LocationName name = LocationName.Create("A01.01").Payload!;
        return Location.Create(name);
    }   
    
}