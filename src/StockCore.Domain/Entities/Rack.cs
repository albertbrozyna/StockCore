using StockCore.Domain.Exceptions;
using StockCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Entities
{
    public class Rack : ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string RackNumber { get; set; }
        public List<Shelf> Shelves { get; set; } = new();
        public Guid WarehouseHallId { get;private set; }
        public int NumberOfLevels { get; private set; }
        public int NumberOfSections { get; private set; }
        
        private Rack() { }
        public Rack(Guid id,Guid tenantId,Guid warehouseHallId, string rackNumber,int numberOfLevels, int numberOfSections)
        {
            if(numberOfLevels <= 0) {
                throw new IncorrectNumberOfLevelsException("Number of levels must be greater than 0");
            }
            if(numberOfSections <= 0)
            {
                throw new IncorrectNumberOfSectionsException("Number of sections must be greater than 0");
            }

            TenantId = tenantId;
            Id = id;
            RackNumber = rackNumber;
            WarehouseHallId = warehouseHallId;
            NumberOfLevels = numberOfLevels;
            NumberOfSections = numberOfSections;
            
        }
    }
}
