import type { CalendarEvent } from "../../domain/entities/CalendarEvent";

const NextCalendarEvents = ({
  calendarEvents,
}: {
  calendarEvents: CalendarEvent[];
}) => {
  return (
    <div>
      <ul>
        <li></li>
        {calendarEvents.map((event) => (
          <ul>
            <li key={event.id}>event.title</li>
            <li>event.date</li>
            <li>event.time</li>
            <li>event.location</li>
            <li>event.type</li>
            <li>event.description</li>
          </ul>
        ))}
      </ul>
    </div>
  );
};

export default NextCalendarEvents;
