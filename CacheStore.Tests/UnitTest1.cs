using System;
using System.Collections.Generic;
using Xunit;

namespace CacheStore.Tests
{
    public class CacheStoreRepositoryTests
    {
        [Fact]
        public void GivenCapacityOverflow_WithValidKeysAndValues_ExpectsRemovalWithEventRaised()
        {
            CacheStoreRepository repository = new CacheStoreRepository(1);
            repository.ItemRemoved += (sender, e) =>
            {
                Assert.NotNull(e);
                Assert.NotNull(e.Item);
                Assert.Equal("SomeKey", e.Item.Key);
                Assert.Equal("SomeValue", e.Item.Value);
            };
            repository.Set("SomeKey", "SomeValue");
            repository.Set("SomeCapacityOverflowKey", "SomeCapacityOverflowValue");
        }

        [Fact]
        public void GivenCapacityOverflow_WithValidKeysAndValues_ExpectsCountEqualToCapacity()
        {
            CacheStoreRepository repository = new CacheStoreRepository(1);
            repository.Set("SomeKey", "SomeValue");
            repository.Set("SomeCapacityOverflowKey", "SomeCapacityOverflowValue");
            Assert.Equal(repository.Capacity, repository.Count);
        }

        [Fact]
        public void GivenSetAndThenRemove_WithInvalidKey_ExpectsRemovalFailure()
        {
            CacheStoreRepository repository = new CacheStoreRepository(1);
            repository.Set("SomeKey", "SomeValue");
            bool actualResult = repository.Remove("SomeKey1");
            Assert.False(actualResult);
        }

        [Fact]
        public void GivenSetAndThenRemove_WithValidKey_ExpectsRemovalSuccess()
        {
            CacheStoreRepository repository = new CacheStoreRepository(1);
            repository.Set("SomeKey", "SomeValue");
            bool actualResult = repository.Remove("SomeKey");
            Assert.True(actualResult);
        }

        [Fact]
        public void GivenUpdatingExistingKey_WithNewValue_ExpectsNewValueEquality()
        {
            CacheStoreRepository repository = new CacheStoreRepository(1);
            repository.Set("SomeKey", "SomeValue");
            repository.Set("SomeKey", "SomeNewValue");
            var actualValue = repository.Get("SomeKey");
            Assert.Equal("SomeNewValue", actualValue);
        }

        [Fact]
        public void GivenSet_WithKeyAndValue_ExpectsValidCount()
        {
            CacheStoreRepository repository = new CacheStoreRepository(1);
            repository.Set("SomeKey", "SomeValue");
            Assert.Equal(1, repository.Count);
        }

        [Fact]
        public void GivenSetAndGet_WithValidKeyAndValue_ExpectsValueEquality()
        {
            CacheStoreRepository repository = new CacheStoreRepository(1);
            repository.Set("SomeKey", "SomeValue");
            var actualValue = repository.Get("SomeKey");
            Assert.Equal("SomeValue", actualValue);
        }

        [Fact]
        public void GivenGet_WithNonExistingKey_ExpectsNullValue()
        {
            CacheStoreRepository repository = new CacheStoreRepository(1);
            var actualValue = repository.Get("SomeKey");
            Assert.Null(actualValue);
        }

        [Fact]
        public void GivenGet_WithNullKey_ExpectsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CacheStoreRepository(1).Get(null));
        }

        [Fact]
        public void GivenSet_WithNullKey_ExpectsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CacheStoreRepository(1).Set(null, null));
        }

        [Fact]
        public void GivenRemove_WithNullKey_ExpectsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CacheStoreRepository(1).Remove(null));
        }

        [Fact]
        public void GivenConstruction_WithZeroCapacity_ExpectsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new CacheStoreRepository(0));
        }

        [Fact]
        public void GivenConstruction_WithNegativeCapacity_ExpectsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new CacheStoreRepository(-1));
        }

        [Fact]
        public void GivenConstruction_WithPositiveCapacity_ExpectsSuccess()
        {
            new CacheStoreRepository(1);
        }
    }
}
