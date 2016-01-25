using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity
{
    public abstract class EntityTypeConfigurationBase<T> : EntityTypeConfiguration<T> where T : class
    {
        protected EntityTypeConfigurationBase()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
            
        }
    }
}
