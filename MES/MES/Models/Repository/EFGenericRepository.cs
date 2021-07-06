using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Web;

/// <summary>
/// 實作Entity Framework Generic Repository 的 Class。
/// </summary>
/// <typeparam name="TEntity">EF Model 裡面的Type</typeparam>
public class EFGenericRepository<TEntity> : IRepository<TEntity> , IDisposable
    where TEntity : class
{
    #region 解構子
    private bool disposed = false;
    private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

    /// <summary>
    /// 解構子,實現IDisposable中的Dispose方法
    /// </summary>
    public void Dispose()
    {
        //必須為true
        Dispose(true);
        //通知垃圾回收機制不再調用終端子（析構器）
        GC.SuppressFinalize(this);
    }
    /// <summary>
    /// 解構子
    /// </summary>
    /// <param name="disposing">disposing</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposed) return;
        //解構時要執行的其它程式
        if (disposing)
        {
            handle.Dispose();
        }
        //讓類別知道自己已經被釋放
        disposed = true;
    }
    /// <summary>
    /// EFGenericRepository 解構子
    /// </summary>
    ~EFGenericRepository()
    {
        //必須為false
        Dispose(false);
    }
    #endregion
    #region 建構子
    /// <summary>
    /// 建構EF一個Entity的Repository，需傳入此Entity的Context。
    /// </summary>
    /// <param name="inContext">Entity所在的Context</param>
    public EFGenericRepository(DbContext inContext)
    {
        Context = inContext;
    }
    #endregion
    #region 屬性
    private DbContext Context { get; set; }
    #endregion
    #region 事件
    /// <summary>
    /// 新增一筆資料到資料庫。
    /// </summary>
    /// <param name="entity">要新增到資料的庫的Entity</param>
    public void Create(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
    }

    /// <summary>
    /// 更新一筆Entity內容。
    /// </summary>
    /// <param name="entity">要更新的內容</param>
    public void Update(TEntity entity)
    {
        Context.Entry<TEntity>(entity).State = EntityState.Modified;
    }

    /// <summary>
    /// 更新一筆Entity的內容。只更新有指定的Property。
    /// </summary>
    /// <param name="entity">要更新的內容。</param>
    /// <param name="updateProperties">需要更新的欄位。</param>
    public void Update(TEntity entity, Expression<Func<TEntity, object>>[] updateProperties)
    {
        Context.Configuration.ValidateOnSaveEnabled = false;

        Context.Entry<TEntity>(entity).State = EntityState.Unchanged;

        if (updateProperties != null)
        {
            foreach (var property in updateProperties)
            {
                Context.Entry<TEntity>(entity).Property(property).IsModified = true;
            }
        }
    }

    /// <summary>
    /// 刪除一筆資料內容。
    /// </summary>
    /// <param name="entity">要被刪除的Entity。</param>
    public void Delete(TEntity entity)
    {
        Context.Entry<TEntity>(entity).State = EntityState.Deleted;
    }

    /// <summary>
    /// 儲存異動。
    /// </summary>
    public void SaveChanges()
    {
        Context.SaveChanges();

        // 因為Update 單一model需要先關掉validation，因此重新打開
        if (Context.Configuration.ValidateOnSaveEnabled == false)
        {
            Context.Configuration.ValidateOnSaveEnabled = true;
        }
    }
    #endregion
    #region 方法
    /// <summary>
    /// 取得第一筆符合條件的內容。如果符合條件有多筆，也只取得第一筆。
    /// </summary>
    /// <param name="predicate">要取得的Where條件。</param>
    /// <returns>取得第一筆符合條件的內容。</returns>
    public TEntity Read(Expression<Func<TEntity, bool>> predicate)
    {
        return Context.Set<TEntity>().Where(predicate).FirstOrDefault();
    }

    /// <summary>
    /// 取得Entity全部筆數的IQueryable。
    /// </summary>
    /// <returns>Entity全部筆數的IQueryable。</returns>
    public IQueryable<TEntity> ReadAll()
    {
        return Context.Set<TEntity>().AsQueryable();
    }

    /// <summary>
    /// 取得Entity全部筆數的IQueryable。
    /// </summary>
    /// <param name="predicate">要取得的Where條件。</param>
    /// <returns>Entity全部筆數的IQueryable。</returns>
    public IQueryable<TEntity> ReadAll(Expression<Func<TEntity, bool>> predicate)
    {
        return Context.Set<TEntity>().Where(predicate).AsQueryable();
    }
    #endregion
}