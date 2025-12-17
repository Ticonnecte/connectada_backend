namespace CidConnectada.Entities.Model.Enums
{
    public enum AuthErrorEnum
    {
        User_Or_Password_Incorrect = 1, //Credenciais Incorretas
        Email_Verification_Required, //Conta com email nao verificado
        Phone_Verification_Required, //Conta com email nao verificado
        Device_Id_Required, //device_id não informado na requisição
        Invalid_GUID_In_Device_Id, //string informada no device_id não está no formato de GUID
        Device_Id_Mismatch, //Tentativa de login com Device diferente ou nao verificado, que é a mesma coisa
        Refresh_Token_Expired, //essa da pra adivinhar
        Refresh_Token_Not_Found, //essa da pra adivinhar
        Tenant_Id_Required, //TenantId nao foi passado no Header
        Tenant_Id_Invalid //TennantId informado no header não pode ser convertido em int
    }
}