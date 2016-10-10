﻿using Remotion.Linq;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArangoDB.Client.Query.Clause
{
    public class GraphDirectionExpressionNode : MethodCallExpressionNodeBase
    {
        public static readonly MethodInfo[] SupportedMethods = new[]
                                                           {
                                                                LinqUtility.GetSupportedMethod(()=>QueryableExtensions.InBound<object,object>(null, null)),
                                                                LinqUtility.GetSupportedMethod(()=>QueryableExtensions.OutBound<object,object>(null, null)),
                                                                LinqUtility.GetSupportedMethod(()=>QueryableExtensions.AnyDirection<object,object>(null, null))
                                                           };

        public ConstantExpression Direction { get; private set; }

        public GraphDirectionExpressionNode(MethodCallExpressionParseInfo parseInfo,
            ConstantExpression direction)
            : base(parseInfo)
        {
            Direction = direction;
        }
        
        public override Expression Resolve(ParameterExpression inputParameter, Expression expressionToBeResolved, ClauseGenerationContext clauseGenerationContext)
        {
            LinqUtility.CheckNotNull("inputParameter", inputParameter);
            LinqUtility.CheckNotNull("expressionToBeResolved", expressionToBeResolved);

            return Source.Resolve(inputParameter, expressionToBeResolved, clauseGenerationContext);
        }

        protected override void ApplyNodeSpecificSemantics(QueryModel queryModel, ClauseGenerationContext clauseGenerationContext)
        {
            LinqUtility.CheckNotNull("queryModel", queryModel);

            var traversalClause = queryModel.BodyClauses.Last(b => b is ITraversalClause) as ITraversalClause;

            traversalClause.Direction = Direction;
        }
    }
}
