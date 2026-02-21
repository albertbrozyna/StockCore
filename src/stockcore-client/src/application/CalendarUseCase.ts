import type {EventRepository} from "../domain/repositories/EventRepository";

export class GetCalendarUseCase{
    private eventRepository : EventRepository;

    constructor(eventRepository : EventRepository){
        this.eventRepository = eventRepository;
    }


    async execute(){
        return await this.eventRepository.getNextEvents();
    }

}