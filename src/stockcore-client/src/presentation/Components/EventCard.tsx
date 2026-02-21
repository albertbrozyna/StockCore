import type {CalendarEvent} from "../../domain/entities/CalendarEvent";



const EventCard = ({eventCard} : {eventCard:CalendarEvent}) => {


    return (
        <div className="event-card">
            <h2>{eventCard.title}</h2>
            <p><strong>Data:</strong> {eventCard.date}</p>
            <p><strong>Godzina:</strong> {eventCard.time}</p>
            <p><strong>Lokalizacja:</strong> {eventCard.location}</p>
            <p><strong>Typ:</strong> {eventCard.type}</p>
            <p><strong>Opis:</strong> {eventCard.description}</p>
        </div>
    );
}
export default EventCard;