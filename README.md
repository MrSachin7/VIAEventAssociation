## Background Description :

VIA Event Association (henceforth VEA, not to be confused with VIA!) is a new start-up initiative in Horsens by a few students. Their idea is to rent rooms/locations at VIA University College Campus Horsens to host events. They will pay VIA some rent for the facilities, and charge guests to make a profit.

However, as this is just starting out and in the testing phase, they have decided to leave out the financial part initially. VIA has gracefully agreed to not charge anything, on the condition that VEA cleans up thoroughly after the events. Just to get things started, as VIA is also interested in more students socializing.

VEA will arrange various events, e.g. beer-tasting in the canteen/café area, art exhibition at Hub C, movie nights in class rooms, or the various party-organizers at VIA may find it easier to get help from VEA when arranging parties. And so forth. In short, VEA will host events.

VEA has various locations/rooms available at Campus Horsens at various times, and the availability may change as VIA themselves may need to use the locations at various times. Maybe a room is available for just one night, or for a month to be used multiple times.

What VEA needs is a way to make available information about events, manage these, and the guests. 

## Description from the customer:
First, we will initially not focus on the financial aspect, as we have been given a prototyping-startup-phase by VIA, where we can test out the concept.

The event is obviously the core concept of our business.

All events must have a title, and optionally a description of the event. There is a start-date and -time, and end-date and -time.

Events can be public or private.

If an event is public, any guest can indicate they are attending, given that the maximum number of guests have not been reached.

If an event is private, then a guest must either accept an invitation, or request to join, which will then be approved by the VEA.

An invitation will have a status indicating whether it has been extended to the guest, accepted by the guest, or rejected by the guest.

The request to join an event must include a reason to join (i.e. some description about why the particular guest is super passionate about this event), and a similar request-status, to indicate whether VEA has approved or rejected the request to join an event. If the event is currently fully booked, VEA can mark a join request as “in-queue”, to indicate that the guest may get a spot, if someone cancels their participation.

Finally, events will have a status, as the creation of an event goes through a few stages:

Initially the event is just created, no information needs to actually to be set here. Maybe the exact date is not yet confirmed. Or the location and maximum number of guests. Etc.

An event can then be readied, and here all the information must be correct. And then it can be made active. I.e. guests can find the event on the “event overview”.

Sometimes we need to cancel events, but we may still want to keep it as a history, so the status will just be cancelled, and then no guest can join, request to join, be invited, etc. No data can be modified, essentially the event is locked.

So, through the invitations, and requests to join, and the guests who indicate they will attend an event, we have an overview of who attends.

We obviously also need guests. They must be registered at our system, so that they can join our events. We just need via-email, and name for the guest.

We need to keep track of which locations are available to us.  These locations come in different forms, e.g. rooms (like “C.02.15”), or open spaces like “The Atrium” or “Hub B”, or even outside, like the basketball court. But all locations are at Campus Horsens. So, we need a specific name for the location.

We must know how many people are allowed to be present at a location, so that we can follow the rules from the fire-services.

And we need to know the availability interval of the location, i.e. when we can use it. This is just a from time/date to a time/date. Locations may become available again later, but we won’t know that, so we don’t need a history of availability, nor a future overview of different windows of availability.


      

### Find requirements here : [Requirements](Documentation/Docs/ProjectCase/requirements.pdf)
