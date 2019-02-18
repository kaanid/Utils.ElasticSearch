using Utils.Util.Helpers;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.ElasticSearch.Stores
{
    public class ESStoreBase<TEntity> : IESStores<TEntity> where TEntity : class
    {
        private IElasticSearchClient _client = null;
        public ESStoreBase(IElasticSearchClient client = null)
        {
            _client = client;
        }

        public void Add(TEntity entity,string index, Expression<Func<TEntity, object>> expression, string typeName = null)
        {
            var column = Lambda.GetName(expression);
            Id id2 = GetId(column,entity);

            Add(entity, index, id2,typeName);
        }

        

        public void Add(IEnumerable<TEntity> entities,string index,Expression<Func<TEntity,object>> expression, string typeName = null)
        {
            if (typeName == null)
            {
                typeName = typeof(TEntity).Name.ToLower();
            }
            var c = _client.GetClient();

            var column = Lambda.GetName(expression);

            var response = c.Bulk(bd => bd.CreateMany<TEntity>(entities,(b,t)=>b.Index(index).Id(GetId(column,t)).Type(typeName)));

            ThrowCheckByResponse(response);
        }

        

        public async Task AddAsync(TEntity entity, string index, Expression<Func<TEntity, object>> expression, string typeName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var column = Lambda.GetName(expression);
            Id id2 = GetId(column, entity);

            await AddAsync(entity, index, id2,typeName,cancellationToken);
        }

        public async Task AddAsync(IEnumerable<TEntity> entities, string index, Expression<Func<TEntity, object>> expression, string typeName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (typeName == null)
            {
                typeName = typeof(TEntity).Name.ToLower();
            }
            var c = _client.GetClient();

            var column = Lambda.GetName(expression);

            var response =await c.BulkAsync(bd => bd.CreateMany<TEntity>(entities, (b, t) => b.Index(index).Id(GetId(column, t)).Type(typeName)));

            ThrowCheckByResponse(response);
        }

        public void Add(TEntity entity, string index, Id id=null, string typeName = null)
        {
            if (typeName == null)
            {
                typeName = typeof(TEntity).Name.ToLower();
            }
            var c = _client.GetClient();

            IResponse response = null;
            if (id==null)
            {
                response = c.Index<TEntity>(entity, cd => cd.Index(index).Id(id).Type(typeName));
            }
            else
            {
                response = c.Create<TEntity>(entity, cd => cd.Index(index).Id(id).Type(typeName));
            }
            
            ThrowCheckByResponse(response);
        }

        public async Task AddAsync(TEntity entity, string index, Id id, string typeName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (typeName == null)
            {
                typeName = typeof(TEntity).Name.ToLower();
            }
            var c = _client.GetClient();

            IResponse response = null;
            if (id == null)
            {
                response =await c.IndexAsync<TEntity>(entity, cd => cd.Index(index).Id(id).Type(typeName));
            }
            else
            {
                response = await c.CreateAsync<TEntity>(entity, cd => cd.Index(index).Id(id).Type(typeName));
            }
            
            ThrowCheckByResponse(response);
        }

        public void Remove(object id, string index, string typeName = null)
        {
            var id2 = GetId(id);
            Remove(id2, index, typeName);
        }

        public void Remove(IEnumerable<object> ids, string index, string typeName = null)
        {
            if (typeName == null)
            {
                typeName = typeof(TEntity).Name.ToLower();
            }
            var c = _client.GetClient();

            List<Id> list = new List<Id>();
            foreach(var id in ids)
            {
                list.Add(GetId(id));
            }

            var response=c.Bulk(bd => bd.DeleteMany(list, (bdd, t) => bdd.Index(index).Type(typeName).Id(t)));
            ThrowCheckByResponse(response);
        }

        public void Remove(Id id, string index, string typeName = null)
        {
            if (typeName == null)
            {
                typeName = typeof(TEntity).Name.ToLower();
            }
            var c = _client.GetClient();
            var response = c.Delete(new DeleteRequest(index, typeName, id));
            ThrowCheckByResponse(response);
        }

        public async Task RemoveAsync(Id id, string index, string typeName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (typeName == null)
            {
                typeName = typeof(TEntity).Name.ToLower();
            }
            var c = _client.GetClient();
            var response =await c.DeleteAsync(new DeleteRequest(index, typeName, id));
            ThrowCheckByResponse(response);
        }

        public async Task RemoveAsync(object id, string index, string typeName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var id2 = GetId(id);
            await RemoveAsync(id2, index, typeName, cancellationToken);
        }

        public async Task RemoveAsync(IEnumerable<object> ids, string index, string typeName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<Id> list = new List<Id>();
            foreach (var id in ids)
            {
                list.Add(GetId(id));
            }

            await RemoveAsync(list, index, typeName, cancellationToken);
        }

        public async Task RemoveAsync(IEnumerable<Id> ids, string index, string typeName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (typeName == null)
            {
                typeName = typeof(TEntity).Name.ToLower();
            }

            var c = _client.GetClient();
            var response = await c.BulkAsync(bd => bd.DeleteMany(ids, (bdd, t) => bdd.Index(index).Type(typeName).Id(t)));
            ThrowCheckByResponse(response);
        }

        public void Update(TEntity entity, Expression<Func<TEntity, object>> expression, string index, string typeName = null)
        {
            var column = Lambda.GetName(expression);
            Id id2 = GetId(column, entity);

            Update(entity, id2, index, typeName);
        }

        public void Update(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> expression, string index, string typeName = null)
        {
            if (typeName == null)
            {
                typeName = typeof(TEntity).Name.ToLower();
            }

            var column = Lambda.GetName(expression);

            var c = _client.GetClient();
            var response = c.Bulk(bd => bd.UpdateMany(entities,(bud,t)=>bud.Index(index).Type(typeName).Id(GetId(column,t)).Doc(t)));

            ThrowCheckByResponse(response);
        }

        public async Task UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> expression, string index, string typeName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var column = Lambda.GetName(expression);
            Id id2 = GetId(column, entity);

            await UpdateAsync(entity, id2, index, typeName,cancellationToken);
        }

        public async Task UpdateAsync(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> expression, string index, string typeName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (typeName == null)
            {
                typeName = typeof(TEntity).Name.ToLower();
            }

            var column = Lambda.GetName(expression);

            var c = _client.GetClient();
            var response = await c.BulkAsync(bd => bd.UpdateMany(entities, (bud, t) => bud.Index(index).Type(typeName).Id(GetId(column, t)).Doc(t)));

            ThrowCheckByResponse(response);
        }

        public void Update(TEntity entity, Id id, string index, string typeName = null)
        {
            if (typeName == null)
            {
                typeName = typeof(TEntity).Name.ToLower();
            }

            var c = _client.GetClient();
            var up = new UpdateRequest<TEntity, TEntity>(index, typeName, id);
            up.Doc = entity;

            var response = c.Update<TEntity>(up);

            ThrowCheckByResponse(response);
        }

        public async Task UpdateAsync(TEntity entity, Id id, string index, string typeName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (typeName == null)
            {
                typeName = typeof(TEntity).Name.ToLower();
            }

            var c = _client.GetClient();
            var up = new UpdateRequest<TEntity, TEntity>(index, typeName, id);
            up.Doc = entity;

            var response = await c.UpdateAsync<TEntity>(up);

            ThrowCheckByResponse(response);
        }

        

        #region private
        private void ThrowCheckByResponse(IResponse response)
        {
            if (response == null)
            {
                throw new NullReferenceException("response is null");
            }

            if (!response.ApiCall.Success)
                throw new Exception($"response.ApiCall.Success {response.ApiCall.HttpMethod} {response.ApiCall.HttpStatusCode} {response.ServerError.Error.Reason}", response.ApiCall.OriginalException);

            if (!response.IsValid)
                throw new Exception($"response.IsValid {response.ApiCall.HttpMethod} {response.ApiCall.HttpStatusCode} {response.ServerError.Error.Reason}", response.ApiCall.OriginalException);
        }

        private Id GetId(object id)
        {
            if (id == null)
                throw new ArgumentNullException();

            switch (id.GetType().Name)
            {
                case "Int32":
                case "Int64":
                    return new Id(System.Convert.ToInt64(id));
                case "String":
                    return new Id(System.Convert.ToInt64(id.ToString()));
                case "Guid":
                    return new Id(Guid.Parse(id.ToString()));
                default:
                    throw new NotImplementedException("不支持");

            }
        }

        private Id GetId(string column, TEntity entity)
        {
            var val = entity.GetType().GetProperty(column);
            var obj = val.GetValue(entity);
            if (obj != null)
            {
                return GetId(obj);
            }

            throw new NullReferenceException("val.GetValue(entity) is null");
        }

        #endregion
    }
}
