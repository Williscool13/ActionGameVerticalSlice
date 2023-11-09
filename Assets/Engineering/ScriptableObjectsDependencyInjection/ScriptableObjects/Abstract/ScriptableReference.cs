namespace ScriptableObjectDependencyInjection
{
    /// <summary>
    /// Extensible base class for ScriptableReferences. Children should also have[Serializable] attribute.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ScriptableReference<T>
    {
        public bool UseConstant = true;
        public T ConstantValue;
        public ScriptableVariable<T> Variable;

        public T Value {
            get {
                return UseConstant ? ConstantValue : Variable.Value;
            }
        }
    }
}