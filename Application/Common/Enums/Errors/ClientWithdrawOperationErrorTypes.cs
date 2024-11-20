
namespace Common.Enums.Errors
{
    public enum ClientWithdrawOperationErrorTypes : int
    {
        AmountError = 0x01,
        ClientMissing = 0x02,
        ExchangeMissing = 0x03,
        AddressError = 0x04
    }
}
