
CREATE TABLE "Guests" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Guests" PRIMARY KEY,
    "ProfilePictureUrl" TEXT NULL,
    "Email" TEXT NOT NULL,
    "FirstName" TEXT NOT NULL,
    "LastName" TEXT NOT NULL
);


CREATE TABLE "Locations" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Locations" PRIMARY KEY,
    "LocationMaxGuests" INTEGER NOT NULL,
    "LocationName" TEXT NOT NULL
);


CREATE TABLE "VeaEvents" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_VeaEvents" PRIMARY KEY,
    "StartDateTime" TEXT NULL,
    "EndDateTime" TEXT NULL,
    "LocationId" TEXT NULL,
    "CurrentStatusState" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "MaxGuests" INTEGER NOT NULL,
    "Title" TEXT NOT NULL,
    "Visibility" TEXT NOT NULL,
    CONSTRAINT "FK_VeaEvents_Locations_LocationId" FOREIGN KEY ("LocationId") REFERENCES "Locations" ("Id")
);


CREATE TABLE "EventInvitation" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_EventInvitation" PRIMARY KEY,
    "Status" TEXT NOT NULL,
    "GuestId" TEXT NOT NULL,
    "VeaEventId" TEXT NOT NULL,
    CONSTRAINT "FK_EventInvitation_Guests_GuestId" FOREIGN KEY ("GuestId") REFERENCES "Guests" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_EventInvitation_VeaEvents_VeaEventId" FOREIGN KEY ("VeaEventId") REFERENCES "VeaEvents" ("Id") ON DELETE CASCADE
);


CREATE TABLE "EventParticipation" (
    "EventId" TEXT NOT NULL,
    "GuestId" TEXT NOT NULL,
    CONSTRAINT "PK_EventParticipation" PRIMARY KEY ("EventId", "GuestId"),
    CONSTRAINT "FK_EventParticipation_Guests_GuestId" FOREIGN KEY ("GuestId") REFERENCES "Guests" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_EventParticipation_VeaEvents_EventId" FOREIGN KEY ("EventId") REFERENCES "VeaEvents" ("Id") ON DELETE CASCADE
);


CREATE INDEX "IX_EventInvitation_GuestId" ON "EventInvitation" ("GuestId");


CREATE INDEX "IX_EventInvitation_VeaEventId" ON "EventInvitation" ("VeaEventId");


CREATE INDEX "IX_EventParticipation_GuestId" ON "EventParticipation" ("GuestId");


CREATE INDEX "IX_VeaEvents_LocationId" ON "VeaEvents" ("LocationId");



