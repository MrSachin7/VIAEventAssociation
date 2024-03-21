using UnitTests.Fakes;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;

namespace UnitTests.Common.Factories;

public static class GuestFactory {
    public static Guest GetValidGuest() {
        GuestFirstName firstName = GuestFirstName.Create("Sachin").Payload!;
        GuestLastName lastName = GuestLastName.Create("Baral").Payload!;
        ViaEmail email = ViaEmail.Create("310628@via.dk").Payload!;


        return Guest.Create(firstName, lastName, email, new TestUniqueEmailChecker()).Payload!;
    }


    public static IEnumerable<object[]> GetValidEmails() {
        return new List<object[]>() {
            new object[] {"310628@via.dk"},
            new object[] {"ALHE@via.dk"},
            new object[] {"TRMO@via.dk"},
            new object[] {"MWA@via.dk"},
        };
    }

    public static IEnumerable<object[]> GetInValidEmails() {
        return new List<object[]>() {
            new object[] {"@via.dk"},
            new object[] {"31062@via.dk"},
            new object[] {"A@via.dk"},
            new object[] {"TB@via.dk"},
            new object[] {""},
            new object[] {"   "},
            new object[] {"sachinbaral@gmail.com"},
            new object[] {"sachin@viaa.com"},
            new object[] {"310628@via.com"},
        };
    }

    public static IEnumerable<object[]> GetValidFirstName() {
        return new List<object[]>() {
            new object[] {"Sachin"},
            new object[] {"SachinWithSPacceAtEnd   "},
            new object[] {"  SachinWithspaceAtStart"},
            new object[] {"   SachinWithspaceAtBoth "},
            new object[] {"       LesThnTwentyFiveWithotSpc       "},
            new object[] {"Troels"},
            new object[] {"naMeWithRandomCaPiTal"},
            new object[] {"rAnDoMnAmE"},
            new object[] {"Tc"},
            // A 25 char name
            new object[] {"a".PadRight(25, 'a')},
            new object[] {"a".PadRight(10, 'a')},
            new object[] {"a".PadRight(15, 'a')},
        };
    }

    public static IEnumerable<object[]> GetInValidFirstName() {
        return new List<object[]>() {
            new object[] {""},
            new object[] {"    "},
            new object[] {"o"},
            new object[] {"StringWithNumber12"},
            new object[] {"StringWith  space"},
            new object[] {"StringWithSpecialChar!"},
            new object[] {"a".PadRight(26, 'a')},
            new object[] {"a".PadRight(100, 'a')},
            new object[] {"a".PadRight(50, 'a')},
        };
    }

    public static IEnumerable<object[]> GetValidLastName() {
        return new List<object[]>() {
            new object[] {"Baral"},
            new object[] {"Mortenesen"},
            new object[] {"Tc"},
            // A 25 char name
            new object[] {"a".PadRight(25, 'a')},
            new object[] {"a".PadRight(10, 'a')},
            new object[] {"a".PadRight(15, 'a')},
        };
    }

    public static IEnumerable<object[]> GetInValidLastName() {
        return new List<object[]>() {
            new object[] {""},
            new object[] {"    "},
            new object[] {"o"},
            new object[] {"StringWithNumber12"},
            new object[] {"StringWithSpecialChar!"},
            new object[] {"a".PadRight(26, 'a')},
            new object[] {"a".PadRight(100, 'a')},
            new object[] {"a".PadRight(50, 'a')},
        };
    }

    public static IEnumerable<object[]> GetValidProfileUri() {
        return new List<object[]>() {
            new object[] {"https://commons.wikimedia.org/wiki/Special:MyLanguage/Commons:Wiki_Loves_Folklore_2024"},
            new object[] {"  https://en.wikipedia.org/wiki/File:Image_created_with_a_mobile_phone.png"},
        };
    }

    public static IEnumerable<object[]> GetInvalidProfileUri() {
        return new List<object[]>() {
            new object[] {"ThisISNotAUri"},
            new object[] {""},
            new object[] {"     "},
        };
    }
}