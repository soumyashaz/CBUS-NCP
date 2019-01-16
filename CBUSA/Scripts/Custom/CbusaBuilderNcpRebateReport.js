function redirecttohistory(contractid)
{
    var url = AjaxCallUrl.RedirectContractUrl.replace("****ContractId****", contractid);
    window.location.href = url;
}

