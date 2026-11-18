using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Enums;

namespace Warehouse.UnitTests.SharedKernel.Enums;

public sealed class SmartEnumTests
{
    [Fact]
    public void DocumentType_has_six_instances_with_expected_codes()
    {
        Enumeration.GetAll<DocumentType>().Select(t => t.Code)
            .ShouldBe(["PZ", "WZ", "MM", "PW", "RW", "KOR"]);
    }

    [Fact]
    public void FromId_and_FromName_resolve_instances_and_reject_unknown_values()
    {
        Enumeration.FromId<DocumentType>(3).ShouldBe(DocumentType.Transfer);
        Enumeration.FromName<DocumentType>("receipt").ShouldBe(DocumentType.Receipt);
        Should.Throw<ArgumentException>(() => Enumeration.FromId<DocumentType>(99));
        Should.Throw<ArgumentException>(() => Enumeration.FromName<MovementType>("nope"));
    }

    [Fact]
    public void MovementType_has_all_11_instances_with_direction_and_sign()
    {
        Enumeration.GetAll<MovementType>().Count.ShouldBe(11);
        MovementType.ReceiptExternal.Direction.ShouldBe(MovementDirection.In);
        MovementType.ReceiptExternal.Sign.ShouldBe(1);
        MovementType.IssueExternal.Sign.ShouldBe(-1);
        MovementType.TransferOut.AffectsValuation.ShouldBeFalse();
        MovementType.TransferIn.AffectsValuation.ShouldBeFalse();
        Enumeration.GetAll<MovementType>().Where(t => t != MovementType.TransferIn && t != MovementType.TransferOut)
            .ShouldAllBe(t => t.AffectsValuation);
    }

    [Fact]
    public void MovementType_Reversal_takes_the_opposite_of_the_reversed_direction()
    {
        MovementType.Reversal.Direction.ShouldBeNull();
        MovementType.Reversal.Sign.ShouldBeNull();
        MovementType.Reversal.ResolveDirection(MovementDirection.In).ShouldBe(MovementDirection.Out);
        MovementType.Reversal.ResolveDirection(MovementDirection.Out).ShouldBe(MovementDirection.In);
        Should.Throw<InvalidOperationException>(() => MovementType.Reversal.ResolveDirection());
        MovementType.IssueExternal.ResolveDirection().ShouldBe(MovementDirection.Out);
    }

    [Theory]
    [InlineData(1, 2, true)]   // Draft -> Approved
    [InlineData(1, 5, true)]   // Draft -> Cancelled
    [InlineData(2, 3, true)]   // Approved -> InTransit
    [InlineData(3, 4, true)]   // InTransit -> Completed
    [InlineData(2, 5, false)]  // approved cannot be cancelled
    [InlineData(2, 1, false)]
    [InlineData(1, 4, false)]
    [InlineData(4, 1, false)]  // terminal
    [InlineData(5, 1, false)]  // terminal
    public void DocumentStatus_transitions_follow_the_state_machine(int from, int to, bool allowed)
    {
        Enumeration.FromId<DocumentStatus>(from).CanTransitionTo(Enumeration.FromId<DocumentStatus>(to)).ShouldBe(allowed);
    }

    [Fact]
    public void ContractorKind_is_a_flags_enum()
    {
        var both = ContractorKind.Supplier | ContractorKind.Customer;

        both.HasFlag(ContractorKind.Customer).ShouldBeTrue();
        ((int)both).ShouldBe(3);
    }
}
