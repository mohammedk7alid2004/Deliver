
using Deliver.BLL.DTOs.Supplier;

namespace Deliver.BLL.Services;

public class SupplierServices(UserManager<ApplicationUser>userManager , IUnitOfWork unitOfWork) : ISupplierServices
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> CreateSupplierAsync(SupplierRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.ApplicationUserId.ToString());
        if (user == null)
            return Result.Failure(UserErrors.UserNotFound);
        var subCategory = await _unitOfWork.SubCategories.GetByIdAsync(request.SubCategoryId);
        if(subCategory == null)
            return  Result.Failure(CategoryError.SubCategoryNotFound);
        var supplier = request.Adapt<Supplier>();
        user.FirstName = request.OwnerName;
        user.PhoneNumber = request.PhoneNumber;
        await _userManager.UpdateAsync(user);
        await  _unitOfWork.Suppliers.AddAsync(supplier);
        await _unitOfWork.SaveAsync();
        return Result.Success(supplier);
    } 

    public Task<bool> DeleteSupplierAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<SupplierResponse>> GetSupplierByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> UpdateSupplierAsync(int id, SupplierRequest request)
    {
        throw new NotImplementedException();
    }
}
