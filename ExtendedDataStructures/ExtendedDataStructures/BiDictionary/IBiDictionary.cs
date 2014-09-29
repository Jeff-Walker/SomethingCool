using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedDataStructures.BiDictionary {
    /// <summary>
    /// A BiDictionary (or "bidirectional dictionary") is a Dictionary that preserves the 
    /// uniquness between its values as well as its keys. This allow it to provide an inverse
    /// view, which is another BiDictionary, but with values and keys reversed. Any changes to
    /// the inverse "view" will happen to the original and vice a versa. It is the caller's 
    /// responsibility to ensure both type parameters provide a proper hash code.
    /// </summary>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public interface IBiDictionary<TK,TV> : IDictionary<TK,TV> {
        /// <summary>
        /// Returns the inverse of this BiDictionary. It mapps the values to the keys.
        /// </summary>
        IBiDictionary<TV, TK> Inverse { get; }

        /// <summary>
        ///  Adds an element with the provided key and value to the <see cref="IBiDictionary{TK,TV}"/>. 
        ///  Will fail if the value is already mapped to another key.
        /// </summary>
        /// <exception cref="T:System.ArgumentException">Thrown if the value is already mapped to another key.</exception>
        /// <param name="key"></param>
        /// <param name="value"></param>
        new void Add(TK key, TV value);

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="IBiDictionary{TK,TV}"/>.
        /// An alternate form of <code>Add(TK,TV)</code> that silently removes any existing entry with the value value before proceeding with the <c>Add(K, V)</c> operation.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void ForceAdd(TK key, TV value);
    }
}
