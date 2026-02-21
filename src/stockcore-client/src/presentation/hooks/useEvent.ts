import type { EventRepository } from "../../domain/repositories/EventRepository"

export const useEvent()=>{



    // TODO USE here dependency injection intstead of creating in manually
    
const repository = new EventRepository();


}