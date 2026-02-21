import type {CalendarEvent} from "../entities/CalendarEvent";


export interface CreateEventRequest {
    title: string;
    date: string;
    time: string;
    location: string;
    type: string;
    description: string;
}

export interface EventRepository{
    getNextEvents():Promise<CalendarEvent[]>;
    createEvent(data: CreateEventRequest):Promise<void>;

}