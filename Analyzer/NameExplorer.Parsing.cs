namespace ConsistencyAnalyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Represents an object that provides info about namees.
    /// </summary>
    public partial class NameExplorer
    {
        #region Init
        private void ParseCompilationUnit(CompilationUnitSyntax compilationUnit)
        {
            foreach (MemberDeclarationSyntax MemberDeclaration in compilationUnit.Members)
                ParseMemberDeclaration(MemberDeclaration);
        }

        private void ParseMemberDeclaration(MemberDeclarationSyntax memberDeclaration)
        {
            switch (memberDeclaration)
            {
                case BaseFieldDeclarationSyntax AsBaseFieldDeclaration:
                    ParseBaseFieldDeclaration(AsBaseFieldDeclaration);
                    break;
                case BaseMethodDeclarationSyntax AsBaseMethodDeclaration:
                    ParseBaseMethodDeclaration(AsBaseMethodDeclaration);
                    break;
                case BasePropertyDeclarationSyntax AsBasePropertyDeclaration:
                    ParseBasePropertyDeclaration(AsBasePropertyDeclaration);
                    break;
                case BaseTypeDeclarationSyntax AsBaseTypeDeclaration:
                    ParseBaseTypeDeclaration(AsBaseTypeDeclaration);
                    break;
                case DelegateDeclarationSyntax AsDelegateDeclaration:
                    ParseDelegateDeclaration(AsDelegateDeclaration);
                    break;
                case EnumMemberDeclarationSyntax AsEnumMemberDeclaration:
                    ParseEnumMemberDeclaration(AsEnumMemberDeclaration);
                    break;
                case GlobalStatementSyntax AsGlobalStatement:
                    ParseGlobalStatement(AsGlobalStatement);
                    break;
                case IncompleteMemberSyntax AsIncompleteMember:
                    ParseIncompleteMember(AsIncompleteMember);
                    break;
                case NamespaceDeclarationSyntax AsNamespaceDeclaration:
                    ParseNamespaceDeclaration(AsNamespaceDeclaration);
                    break;
            }
        }

        private void ParseBaseFieldDeclaration(BaseFieldDeclarationSyntax baseFieldDeclaration)
        {
            switch (baseFieldDeclaration)
            {
                case EventFieldDeclarationSyntax AsEventFieldDeclaration:
                    ParseEventFieldDeclaration(AsEventFieldDeclaration);
                    break;
                case FieldDeclarationSyntax AsFieldDeclaration:
                    ParseFieldDeclaration(AsFieldDeclaration);
                    break;
            }
        }

        private void ParseBaseMethodDeclaration(BaseMethodDeclarationSyntax baseMethodDeclaration)
        {
            switch (baseMethodDeclaration)
            {
                case ConstructorDeclarationSyntax AsConstructorDeclaration:
                    ParseConstructorDeclaration(AsConstructorDeclaration);
                    break;
                case ConversionOperatorDeclarationSyntax AsConversionOperatorDeclaration:
                    ParseConversionOperatorDeclaration(AsConversionOperatorDeclaration);
                    break;
                case DestructorDeclarationSyntax AsDestructorDeclaration:
                    ParseDestructorDeclaration(AsDestructorDeclaration);
                    break;
                case MethodDeclarationSyntax AsMethodDeclaration:
                    ParseMethodDeclaration(AsMethodDeclaration);
                    break;
                case OperatorDeclarationSyntax AsOperatorDeclaration:
                    ParseOperatorDeclaration(AsOperatorDeclaration);
                    break;
            }
        }

        private void ParseBasePropertyDeclaration(BasePropertyDeclarationSyntax basePropertyDeclaration)
        {
            switch (basePropertyDeclaration)
            {
                case EventDeclarationSyntax AsEventDeclaration:
                    ParseEventDeclaration(AsEventDeclaration);
                    break;
                case IndexerDeclarationSyntax AsIndexerDeclaration:
                    ParseIndexerDeclaration(AsIndexerDeclaration);
                    break;
                case PropertyDeclarationSyntax AsPropertyDeclaration:
                    ParsePropertyDeclaration(AsPropertyDeclaration);
                    break;
            }
        }

        private void ParseBaseTypeDeclaration(BaseTypeDeclarationSyntax baseTypeDeclaration)
        {
            switch (baseTypeDeclaration)
            {
                case EnumDeclarationSyntax AsEnumDeclaration:
                    ParseEnumDeclaration(AsEnumDeclaration);
                    break;
                case TypeDeclarationSyntax AsTypeDeclaration:
                    ParseTypeDeclaration(AsTypeDeclaration);
                    break;
            }
        }

        private void ParseTypeDeclaration(TypeDeclarationSyntax typeDeclaration)
        {
            switch (typeDeclaration)
            {
                case ClassDeclarationSyntax AsClassDeclaration:
                    ParseClassDeclaration(AsClassDeclaration);
                    break;
                case InterfaceDeclarationSyntax AsInterfaceDeclaration:
                    ParseInterfaceDeclaration(AsInterfaceDeclaration);
                    break;
                case RecordDeclarationSyntax AsRecordDeclaration:
                    ParseRecordDeclaration(AsRecordDeclaration);
                    break;
                case StructDeclarationSyntax AsStructDeclaration:
                    ParseStructDeclaration(AsStructDeclaration);
                    break;
            }
        }

        private void ParseEventFieldDeclaration(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            VariableDeclarationSyntax VariableDeclaration = eventFieldDeclaration.Declaration;
            ParseVariableDeclaration(VariableDeclaration, NameCategory.Event);
        }

        private void ParseFieldDeclaration(FieldDeclarationSyntax fieldDeclaration)
        {
            VariableDeclarationSyntax VariableDeclaration = fieldDeclaration.Declaration;
            ParseVariableDeclaration(VariableDeclaration, NameCategory.Field);
        }

        private void ParseConstructorDeclaration(ConstructorDeclarationSyntax constructorDeclaration)
        {
            if (constructorDeclaration.Body != null)
                ParseBlock(constructorDeclaration.Body);
            if (constructorDeclaration.Initializer != null)
                ParseConstructorInitializer(constructorDeclaration.Initializer);
            ParseParameterList(constructorDeclaration.ParameterList);
            if (constructorDeclaration.ExpressionBody != null)
                ParseArrowExpressionClause(constructorDeclaration.ExpressionBody);
        }

        private void ParseConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            if (conversionOperatorDeclaration.Body != null)
                ParseBlock(conversionOperatorDeclaration.Body);
            ParseParameterList(conversionOperatorDeclaration.ParameterList);
            ParseType(conversionOperatorDeclaration.Type);
            if (conversionOperatorDeclaration.ExpressionBody != null)
                ParseArrowExpressionClause(conversionOperatorDeclaration.ExpressionBody);
        }

        private void ParseDestructorDeclaration(DestructorDeclarationSyntax destructorDeclaration)
        {
            if (destructorDeclaration.Body != null)
                ParseBlock(destructorDeclaration.Body);
            ParseParameterList(destructorDeclaration.ParameterList);
            if (destructorDeclaration.ExpressionBody != null)
                ParseArrowExpressionClause(destructorDeclaration.ExpressionBody);
        }

        private void ParseMethodDeclaration(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration.ExpressionBody != null)
                ParseArrowExpressionClause(methodDeclaration.ExpressionBody);
            if (methodDeclaration.Body != null)
                ParseBlock(methodDeclaration.Body);
            foreach (TypeParameterConstraintClauseSyntax TypeParameterConstraintClause in methodDeclaration.ConstraintClauses)
                ParseTypeParameterConstraintClause(TypeParameterConstraintClause);
            ParseParameterList(methodDeclaration.ParameterList);
            if (methodDeclaration.TypeParameterList != null)
                ParseTypeParameterList(methodDeclaration.TypeParameterList);
            ParseIdentifier(methodDeclaration.Identifier, NameCategory.Method);
            if (methodDeclaration.ExplicitInterfaceSpecifier != null)
                ParseExplicitInterfaceSpecifier(methodDeclaration.ExplicitInterfaceSpecifier);
            ParseType(methodDeclaration.ReturnType);
        }

        private void ParseOperatorDeclaration(OperatorDeclarationSyntax operatorDeclaration)
        {
            if (operatorDeclaration.Body != null)
                ParseBlock(operatorDeclaration.Body);
            ParseParameterList(operatorDeclaration.ParameterList);
            ParseType(operatorDeclaration.ReturnType);
            if (operatorDeclaration.ExpressionBody != null)
                ParseArrowExpressionClause(operatorDeclaration.ExpressionBody);
        }

        private void ParseEventDeclaration(EventDeclarationSyntax eventDeclaration)
        {
            ParseIdentifier(eventDeclaration.Identifier, NameCategory.Event);
            if (eventDeclaration.ExplicitInterfaceSpecifier != null)
                ParseExplicitInterfaceSpecifier(eventDeclaration.ExplicitInterfaceSpecifier);
            ParseType(eventDeclaration.Type);
            if (eventDeclaration.AccessorList != null)
                ParseAccessorList(eventDeclaration.AccessorList);
        }

        private void ParseIndexerDeclaration(IndexerDeclarationSyntax indexerDeclaration)
        {
            if (indexerDeclaration.ExpressionBody != null)
                ParseArrowExpressionClause(indexerDeclaration.ExpressionBody);
            if (indexerDeclaration.AccessorList != null)
                ParseAccessorList(indexerDeclaration.AccessorList);
            ParseBracketedParameterList(indexerDeclaration.ParameterList);
            if (indexerDeclaration.ExplicitInterfaceSpecifier != null)
                ParseExplicitInterfaceSpecifier(indexerDeclaration.ExplicitInterfaceSpecifier);
            ParseType(indexerDeclaration.Type);
        }

        private void ParsePropertyDeclaration(PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration.Initializer != null)
                ParseEqualsValueClause(propertyDeclaration.Initializer);
            if (propertyDeclaration.ExpressionBody != null)
                ParseArrowExpressionClause(propertyDeclaration.ExpressionBody);
            if (propertyDeclaration.AccessorList != null)
                ParseAccessorList(propertyDeclaration.AccessorList);
            ParseIdentifier(propertyDeclaration.Identifier, NameCategory.Property);
            if (propertyDeclaration.ExplicitInterfaceSpecifier != null)
                ParseExplicitInterfaceSpecifier(propertyDeclaration.ExplicitInterfaceSpecifier);
            ParseType(propertyDeclaration.Type);
        }

        private void ParseEnumDeclaration(EnumDeclarationSyntax enumDeclaration)
        {
            foreach (EnumMemberDeclarationSyntax EnumMemberDeclaration in enumDeclaration.Members)
                ParseEnumMemberDeclaration(EnumMemberDeclaration);
            if (enumDeclaration.BaseList != null)
                ParseBaseList(enumDeclaration.BaseList);
            ParseIdentifier(enumDeclaration.Identifier, NameCategory.Enum);
        }

        private void ParseClassDeclaration(ClassDeclarationSyntax classDeclaration)
        {
            foreach (MemberDeclarationSyntax MemberDeclaration in classDeclaration.Members)
                ParseMemberDeclaration(MemberDeclaration);
            foreach (TypeParameterConstraintClauseSyntax TypeParameterConstraintClause in classDeclaration.ConstraintClauses)
                ParseTypeParameterConstraintClause(TypeParameterConstraintClause);
            if (classDeclaration.BaseList != null)
                ParseBaseList(classDeclaration.BaseList);
            if (classDeclaration.TypeParameterList != null)
                ParseTypeParameterList(classDeclaration.TypeParameterList);
            ParseIdentifier(classDeclaration.Identifier, NameCategory.Class);
        }

        private void ParseInterfaceDeclaration(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            foreach (MemberDeclarationSyntax MemberDeclaration in interfaceDeclaration.Members)
                ParseMemberDeclaration(MemberDeclaration);
            foreach (TypeParameterConstraintClauseSyntax TypeParameterConstraintClause in interfaceDeclaration.ConstraintClauses)
                ParseTypeParameterConstraintClause(TypeParameterConstraintClause);
            if (interfaceDeclaration.BaseList != null)
                ParseBaseList(interfaceDeclaration.BaseList);
            if (interfaceDeclaration.TypeParameterList != null)
                ParseTypeParameterList(interfaceDeclaration.TypeParameterList);
            ParseIdentifier(interfaceDeclaration.Identifier, NameCategory.Interface);
        }

        private void ParseRecordDeclaration(RecordDeclarationSyntax recordDeclaration)
        {
            foreach (MemberDeclarationSyntax MemberDeclaration in recordDeclaration.Members)
                ParseMemberDeclaration(MemberDeclaration);
            foreach (TypeParameterConstraintClauseSyntax TypeParameterConstraintClause in recordDeclaration.ConstraintClauses)
                ParseTypeParameterConstraintClause(TypeParameterConstraintClause);
            if (recordDeclaration.BaseList != null)
                ParseBaseList(recordDeclaration.BaseList);
            if (recordDeclaration.ParameterList != null)
                ParseParameterList(recordDeclaration.ParameterList);
            if (recordDeclaration.TypeParameterList != null)
                ParseTypeParameterList(recordDeclaration.TypeParameterList);
            ParseIdentifier(recordDeclaration.Identifier, NameCategory.Record);
        }

        private void ParseStructDeclaration(StructDeclarationSyntax structDeclaration)
        {
            foreach (MemberDeclarationSyntax MemberDeclaration in structDeclaration.Members)
                ParseMemberDeclaration(MemberDeclaration);
            foreach (TypeParameterConstraintClauseSyntax TypeParameterConstraintClause in structDeclaration.ConstraintClauses)
                ParseTypeParameterConstraintClause(TypeParameterConstraintClause);
            if (structDeclaration.BaseList != null)
                ParseBaseList(structDeclaration.BaseList);
            if (structDeclaration.TypeParameterList != null)
                ParseTypeParameterList(structDeclaration.TypeParameterList);
            ParseIdentifier(structDeclaration.Identifier, NameCategory.Struct);
        }

        private void ParseDelegateDeclaration(DelegateDeclarationSyntax delegateDeclaration)
        {
            foreach (TypeParameterConstraintClauseSyntax TypeParameterConstraintClause in delegateDeclaration.ConstraintClauses)
                ParseTypeParameterConstraintClause(TypeParameterConstraintClause);
            ParseParameterList(delegateDeclaration.ParameterList);
            if (delegateDeclaration.TypeParameterList != null)
                ParseTypeParameterList(delegateDeclaration.TypeParameterList);
            ParseIdentifier(delegateDeclaration.Identifier, NameCategory.Delegate);
        }

        private void ParseEnumMemberDeclaration(EnumMemberDeclarationSyntax enumMemberDeclaration)
        {
            ParseIdentifier(enumMemberDeclaration.Identifier, NameCategory.EnumMember);
        }

        private void ParseGlobalStatement(GlobalStatementSyntax globalStatement)
        {
            ParseStatement(globalStatement.Statement);
        }

        private void ParseIncompleteMember(IncompleteMemberSyntax incompleteMember)
        {
            if (incompleteMember.Type != null)
                ParseType(incompleteMember.Type);
        }

        private void ParseNamespaceDeclaration(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            foreach (MemberDeclarationSyntax MemberDeclaration in namespaceDeclaration.Members)
                ParseMemberDeclaration(MemberDeclaration);
            ParseName(namespaceDeclaration.Name, NameCategory.Namespace);
        }

        private void ParseVariableDeclaration(VariableDeclarationSyntax variableDeclaration, NameCategory nameCategory)
        {
            ParseType(variableDeclaration.Type);
            foreach (VariableDeclaratorSyntax VariableDeclarator in variableDeclaration.Variables)
                ParseVariableDeclarator(VariableDeclarator, nameCategory);
        }

        private void ParseBlock(BlockSyntax body)
        {
            foreach (StatementSyntax Statement in body.Statements)
                ParseStatement(Statement);
        }

        private void ParseConstructorInitializer(ConstructorInitializerSyntax constructorInitializer)
        {
            ParseArgumentList(constructorInitializer.ArgumentList);
        }

        private void ParseParameterList(ParameterListSyntax parameterList)
        {
            foreach (ParameterSyntax Parameter in parameterList.Parameters)
                ParseParameter(Parameter);
        }

        private void ParseParameter(ParameterSyntax parameter)
        {
            if (parameter.Type != null)
                ParseType(parameter.Type);
            ParseIdentifier(parameter.Identifier, NameCategory.Parameter);
            if (parameter.Default != null)
                ParseEqualsValueClause(parameter.Default);
        }

        private void ParseArgumentList(ArgumentListSyntax argumentList)
        {
            foreach (ArgumentSyntax Argument in argumentList.Arguments)
                ParseArgument(Argument);
        }

        private void ParseArgument(ArgumentSyntax argument)
        {
            if (argument.NameColon != null)
                ParseNameColon(argument.NameColon);
            ParseExpression(argument.Expression);
        }

        private void ParseArrowExpressionClause(ArrowExpressionClauseSyntax expressionBody)
        {
            ParseExpression(expressionBody.Expression);
        }

        private void ParseType(TypeSyntax type)
        {
            switch (type)
            {
                case ArrayTypeSyntax AsArrayType:
                    ParseArrayType(AsArrayType);
                    break;
                case FunctionPointerTypeSyntax AsFunctionPointerType:
                    ParseFunctionPointerType(AsFunctionPointerType);
                    break;
                case NameSyntax AsName:
                    ParseName(AsName, NameCategory.Neutral);
                    break;
                case NullableTypeSyntax AsNullableType:
                    ParseNullableType(AsNullableType);
                    break;
                case OmittedTypeArgumentSyntax _:
                    break;
                case PointerTypeSyntax AsPointerType:
                    ParsePointerType(AsPointerType);
                    break;
                case PredefinedTypeSyntax _:
                    break;
                case RefTypeSyntax AsRefType:
                    ParseRefType(AsRefType);
                    break;
                case TupleTypeSyntax AsTupleType:
                    ParseTupleType(AsTupleType);
                    break;
            }
        }

        private void ParseArrayType(ArrayTypeSyntax arrayType)
        {
            ParseType(arrayType.ElementType);
            foreach (ArrayRankSpecifierSyntax ArrayRankSpecifier in arrayType.RankSpecifiers)
                ParseArrayRankSpecifier(ArrayRankSpecifier);
        }

        private void ParseFunctionPointerType(FunctionPointerTypeSyntax functionPointerType)
        {
            if (functionPointerType.CallingConvention != null)
                ParseFunctionPointerCallingConvention(functionPointerType.CallingConvention);
            ParseFunctionPointerParameterList(functionPointerType.ParameterList);
        }

        private void ParseName(NameSyntax name, NameCategory nameCategory)
        {
            switch (name)
            {
                case AliasQualifiedNameSyntax AsAliasQualifiedName:
                    ParseAliasQualifiedName(AsAliasQualifiedName, nameCategory);
                    break;
                case QualifiedNameSyntax AsQualifiedName:
                    ParseQualifiedName(AsQualifiedName, nameCategory);
                    break;
                case SimpleNameSyntax AsSimpleName:
                    ParseSimpleName(AsSimpleName, nameCategory);
                    break;
            }
        }

        private void ParseAliasQualifiedName(AliasQualifiedNameSyntax aliasQualifiedName, NameCategory nameCategory)
        {
            ParseIdentifierName(aliasQualifiedName.Alias, nameCategory);
            ParseSimpleName(aliasQualifiedName.Name, nameCategory);
        }

        private void ParseQualifiedName(QualifiedNameSyntax qualifiedName, NameCategory nameCategory)
        {
            ParseName(qualifiedName.Left, NameCategory.Namespace);
            ParseSimpleName(qualifiedName.Right, nameCategory);
        }

        private void ParseSimpleName(SimpleNameSyntax simpleName, NameCategory nameCategory)
        {
            switch (simpleName)
            {
                case GenericNameSyntax AsGenericName:
                    ParseGenericName(AsGenericName);
                    break;
                case IdentifierNameSyntax AsIdentifierName:
                    ParseIdentifierName(AsIdentifierName, nameCategory);
                    break;
            }
        }

        private void ParseGenericName(GenericNameSyntax genericName)
        {
            ParseIdentifier(genericName.Identifier, NameCategory.Neutral);
            ParseTypeArgumentList(genericName.TypeArgumentList);
        }

        private void ParseIdentifierName(IdentifierNameSyntax identifierName, NameCategory nameCategory)
        {
            ParseIdentifier(identifierName.Identifier, nameCategory);
        }

        private void ParseNullableType(NullableTypeSyntax nullableType)
        {
            ParseType(nullableType.ElementType);
        }

        private void ParsePointerType(PointerTypeSyntax pointerType)
        {
            ParseType(pointerType.ElementType);
        }

        private void ParseRefType(RefTypeSyntax refType)
        {
            ParseType(refType.Type);
        }

        private void ParseTupleType(TupleTypeSyntax tupleType)
        {
            foreach (TupleElementSyntax TupleElement in tupleType.Elements)
                ParseTupleElement(TupleElement);
        }

        private void ParseTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax typeParameterConstraintClause)
        {
            ParseIdentifierName(typeParameterConstraintClause.Name, NameCategory.Neutral);
            foreach (TypeParameterConstraintSyntax TypeParameterConstraint in typeParameterConstraintClause.Constraints)
                ParseTypeParameterConstraint(TypeParameterConstraint);
        }

        private void ParseTypeParameterList(TypeParameterListSyntax typeParameterList)
        {
            foreach (TypeParameterSyntax TypeParameter in typeParameterList.Parameters)
                ParseTypeParameter(TypeParameter);
        }

        private void ParseExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier)
        {
            ParseName(explicitInterfaceSpecifier.Name, NameCategory.Neutral);
        }

        private void ParseAccessorList(AccessorListSyntax accessorList)
        {
            foreach (AccessorDeclarationSyntax AccessorDeclaration in accessorList.Accessors)
                ParseAccessorDeclaration(AccessorDeclaration);
        }

        private void ParseBracketedParameterList(BracketedParameterListSyntax bracketedParameterList)
        {
            foreach (ParameterSyntax Parameter in bracketedParameterList.Parameters)
                ParseParameter(Parameter);
        }

        private void ParseBracketedArgumentList(BracketedArgumentListSyntax bracketedArgumentList)
        {
            foreach (ArgumentSyntax Argument in bracketedArgumentList.Arguments)
                ParseArgument(Argument);
        }

        private void ParseEqualsValueClause(EqualsValueClauseSyntax equalsValueClause)
        {
            ParseExpression(equalsValueClause.Value);
        }

        private void ParseBaseList(BaseListSyntax baseList)
        {
            foreach (BaseTypeSyntax BaseType in baseList.Types)
                ParseBaseType(BaseType);
        }

        private void ParseVariableDeclarator(VariableDeclaratorSyntax variableDeclarator, NameCategory nameCategory)
        {
            ParseIdentifier(variableDeclarator.Identifier, nameCategory);
            if (variableDeclarator.ArgumentList != null)
                ParseBracketedArgumentList(variableDeclarator.ArgumentList);
            if (variableDeclarator.Initializer != null)
                ParseEqualsValueClause(variableDeclarator.Initializer);
        }

        private void ParseNameColon(NameColonSyntax nameColon)
        {
            ParseIdentifierName(nameColon.Name, NameCategory.Neutral);
        }

        private void ParseArrayRankSpecifier(ArrayRankSpecifierSyntax arrayRankSpecifier)
        {
            foreach (ExpressionSyntax Expression in arrayRankSpecifier.Sizes)
                ParseExpression(Expression);
        }

        private void ParseFunctionPointerCallingConvention(FunctionPointerCallingConventionSyntax functionPointerCallingConvention)
        {
            if (functionPointerCallingConvention.UnmanagedCallingConventionList != null)
                ParseFunctionPointerUnmanagedCallingConventionList(functionPointerCallingConvention.UnmanagedCallingConventionList);
        }

        private void ParseFunctionPointerParameterList(FunctionPointerParameterListSyntax functionPointerParameterList)
        {
            foreach (FunctionPointerParameterSyntax FunctionPointerParameter in functionPointerParameterList.Parameters)
                ParseFunctionPointerParameter(FunctionPointerParameter);
        }

        private void ParseTypeArgumentList(TypeArgumentListSyntax typeArgumentListSyntax)
        {
            foreach (TypeSyntax Type in typeArgumentListSyntax.Arguments)
                ParseType(Type);
        }

        private void ParseTupleElement(TupleElementSyntax tupleElement)
        {
            ParseType(tupleElement.Type);
            ParseIdentifier(tupleElement.Identifier, NameCategory.Field);
        }

        private void ParseTypeParameterConstraint(TypeParameterConstraintSyntax typeParameterConstraint)
        {
            switch (typeParameterConstraint)
            {
                case ClassOrStructConstraintSyntax _:
                    break;
                case ConstructorConstraintSyntax _:
                    break;
                case TypeConstraintSyntax AsTypeConstraint:
                    ParseTypeConstraint(AsTypeConstraint);
                    break;
            }
        }

        private void ParseTypeConstraint(TypeConstraintSyntax typeConstraint)
        {
            ParseType(typeConstraint.Type);
        }

        private void ParseTypeParameter(TypeParameterSyntax typeParameter)
        {
            ParseIdentifier(typeParameter.Identifier, NameCategory.TypeParameter);
        }

        private void ParseAccessorDeclaration(AccessorDeclarationSyntax accessorDeclaration)
        {
            if (accessorDeclaration.Body != null)
                ParseBlock(accessorDeclaration.Body);
            if (accessorDeclaration.ExpressionBody != null)
                ParseArrowExpressionClause(accessorDeclaration.ExpressionBody);
        }

        private void ParseBaseType(BaseTypeSyntax baseType)
        {
            switch (baseType)
            {
                case PrimaryConstructorBaseTypeSyntax AsPrimaryConstructorBaseType:
                    ParsePrimaryConstructorBaseType(AsPrimaryConstructorBaseType);
                    break;
                case SimpleBaseTypeSyntax AsSimpleBaseType:
                    ParseSimpleBaseType(AsSimpleBaseType);
                    break;
            }
        }

        private void ParsePrimaryConstructorBaseType(PrimaryConstructorBaseTypeSyntax primaryConstructorBaseType)
        {
            ParseType(primaryConstructorBaseType.Type);
            ParseArgumentList(primaryConstructorBaseType.ArgumentList);
        }

        private void ParseSimpleBaseType(SimpleBaseTypeSyntax simpleBaseType)
        {
            ParseType(simpleBaseType.Type);
        }

        private void ParseFunctionPointerUnmanagedCallingConventionList(FunctionPointerUnmanagedCallingConventionListSyntax functionPointerUnmanagedCallingConventionList)
        {
            foreach (FunctionPointerUnmanagedCallingConventionSyntax FunctionPointerUnmanagedCallingConvention in functionPointerUnmanagedCallingConventionList.CallingConventions)
                ParseFunctionPointerUnmanagedCallingConvention(FunctionPointerUnmanagedCallingConvention);
        }

        private void ParseFunctionPointerUnmanagedCallingConvention(FunctionPointerUnmanagedCallingConventionSyntax _)
        {
        }

        private void ParseFunctionPointerParameter(FunctionPointerParameterSyntax functionPointerParameter)
        {
            ParseType(functionPointerParameter.Type);
        }

        private void ParseStatement(StatementSyntax statement)
        {
            switch (statement)
            {
                case BlockSyntax AsBlock:
                    ParseBlock(AsBlock);
                    break;
                case BreakStatementSyntax _:
                    break;
                case CheckedStatementSyntax AsCheckedStatement:
                    ParseCheckedStatement(AsCheckedStatement);
                    break;
                case ForEachStatementSyntax AsForEachStatement:
                    ParseForEachStatement(AsForEachStatement);
                    break;
                case ForEachVariableStatementSyntax AsForEachVariableStatement:
                    ParseForEachVariableStatement(AsForEachVariableStatement);
                    break;
                case ContinueStatementSyntax _:
                    break;
                case DoStatementSyntax AsDoStatement:
                    ParseDoStatement(AsDoStatement);
                    break;
                case EmptyStatementSyntax _:
                    break;
                case ExpressionStatementSyntax AsExpressionStatement:
                    ParseExpressionStatement(AsExpressionStatement);
                    break;
                case FixedStatementSyntax AsFixedStatement:
                    ParseFixedStatement(AsFixedStatement);
                    break;
                case ForStatementSyntax AsForStatement:
                    ParseForStatement(AsForStatement);
                    break;
                case GotoStatementSyntax AsGotoStatement:
                    ParseGotoStatement(AsGotoStatement);
                    break;
                case IfStatementSyntax AsIfStatement:
                    ParseIfStatement(AsIfStatement);
                    break;
                case LabeledStatementSyntax AsLabeledStatement:
                    ParseLabeledStatement(AsLabeledStatement);
                    break;
                case LocalDeclarationStatementSyntax AsLocalDeclarationStatement:
                    ParseLocalDeclarationStatement(AsLocalDeclarationStatement);
                    break;
                case LocalFunctionStatementSyntax AsLocalFunctionStatement:
                    ParseLocalFunctionStatement(AsLocalFunctionStatement);
                    break;
                case LockStatementSyntax AsLockStatement:
                    ParseLockStatement(AsLockStatement);
                    break;
                case ReturnStatementSyntax AsReturnStatement:
                    ParseReturnStatement(AsReturnStatement);
                    break;
                case SwitchStatementSyntax AsSwitchStatement:
                    ParseSwitchStatement(AsSwitchStatement);
                    break;
                case ThrowStatementSyntax AsThrowStatement:
                    ParseThrowStatement(AsThrowStatement);
                    break;
                case TryStatementSyntax AsTryStatement:
                    ParseTryStatement(AsTryStatement);
                    break;
                case UnsafeStatementSyntax AsUnsafeStatement:
                    ParseUnsafeStatement(AsUnsafeStatement);
                    break;
                case UsingStatementSyntax AsUsingStatement:
                    ParseUsingStatement(AsUsingStatement);
                    break;
                case WhileStatementSyntax AsWhileStatement:
                    ParseWhileStatement(AsWhileStatement);
                    break;
                case YieldStatementSyntax AsYieldStatement:
                    ParseYieldStatement(AsYieldStatement);
                    break;
            }
        }

        private void ParseCheckedStatement(CheckedStatementSyntax checkedStatement)
        {
            ParseBlock(checkedStatement.Block);
        }

        private void ParseForEachStatement(ForEachStatementSyntax forEachStatement)
        {
            ParseStatement(forEachStatement.Statement);
            ParseExpression(forEachStatement.Expression);
            ParseType(forEachStatement.Type);
        }

        private void ParseForEachVariableStatement(ForEachVariableStatementSyntax forEachVariableStatement)
        {
            ParseStatement(forEachVariableStatement.Statement);
            ParseExpression(forEachVariableStatement.Expression);
            ParseExpression(forEachVariableStatement.Variable);
        }

        private void ParseDoStatement(DoStatementSyntax doStatement)
        {
            ParseExpression(doStatement.Condition);
            ParseStatement(doStatement.Statement);
        }

        private void ParseExpressionStatement(ExpressionStatementSyntax expressionStatement)
        {
            ParseExpression(expressionStatement.Expression);
        }

        private void ParseFixedStatement(FixedStatementSyntax fixedStatement)
        {
            ParseStatement(fixedStatement.Statement);
            ParseVariableDeclaration(fixedStatement.Declaration, NameCategory.LocalVariable);
        }

        private void ParseForStatement(ForStatementSyntax forStatement)
        {
            ParseStatement(forStatement.Statement);
            foreach (ExpressionSyntax Expression in forStatement.Incrementors)
                ParseExpression(Expression);
            if (forStatement.Condition != null)
                ParseExpression(forStatement.Condition);
            foreach (ExpressionSyntax Expression in forStatement.Initializers)
                ParseExpression(Expression);
            if (forStatement.Declaration != null)
                ParseVariableDeclaration(forStatement.Declaration, NameCategory.LocalVariable);
        }

        private void ParseGotoStatement(GotoStatementSyntax gotoStatement)
        {
            if (gotoStatement.Expression != null)
                ParseExpression(gotoStatement.Expression);
        }

        private void ParseIfStatement(IfStatementSyntax ifStatement)
        {
            if (ifStatement.Else != null)
                ParseElseClause(ifStatement.Else);
            ParseExpression(ifStatement.Condition);
            ParseStatement(ifStatement.Statement);
        }

        private void ParseElseClause(ElseClauseSyntax elseClause)
        {
            ParseStatement(elseClause.Statement);
        }

        private void ParseLabeledStatement(LabeledStatementSyntax labeledStatement)
        {
            ParseStatement(labeledStatement.Statement);
        }

        private void ParseLocalDeclarationStatement(LocalDeclarationStatementSyntax localDeclarationStatement)
        {
            ParseVariableDeclaration(localDeclarationStatement.Declaration, NameCategory.LocalVariable);
        }

        private void ParseLocalFunctionStatement(LocalFunctionStatementSyntax localFunctionStatement)
        {
            if (localFunctionStatement.Body != null)
                ParseBlock(localFunctionStatement.Body);
            foreach (TypeParameterConstraintClauseSyntax TypeParameterConstraintClause in localFunctionStatement.ConstraintClauses)
                ParseTypeParameterConstraintClause(TypeParameterConstraintClause);
            ParseParameterList(localFunctionStatement.ParameterList);
            if (localFunctionStatement.ExpressionBody != null)
                ParseArrowExpressionClause(localFunctionStatement.ExpressionBody);
        }

        private void ParseLockStatement(LockStatementSyntax lockStatement)
        {
            ParseStatement(lockStatement.Statement);
            ParseExpression(lockStatement.Expression);
        }

        private void ParseReturnStatement(ReturnStatementSyntax returnStatement)
        {
            if (returnStatement.Expression != null)
                ParseExpression(returnStatement.Expression);
        }

        private void ParseSwitchStatement(SwitchStatementSyntax switchStatement)
        {
            ParseExpression(switchStatement.Expression);
            foreach (SwitchSectionSyntax SwitchSection in switchStatement.Sections)
                ParseSwitchSection(SwitchSection);
        }

        private void ParseSwitchSection(SwitchSectionSyntax switchSection)
        {
            foreach (SwitchLabelSyntax SwitchLabel in switchSection.Labels)
                ParseSwitchLabel(SwitchLabel);
            foreach (StatementSyntax Statement in switchSection.Statements)
                ParseStatement(Statement);
        }

        private void ParseSwitchLabel(SwitchLabelSyntax switchLabel)
        {
            switch (switchLabel)
            {
                case CasePatternSwitchLabelSyntax AsCasePatternSwitchLabel:
                    ParseCasePatternSwitchLabel(AsCasePatternSwitchLabel);
                    break;
                case CaseSwitchLabelSyntax AsCaseSwitchLabel:
                    ParseCaseSwitchLabel(AsCaseSwitchLabel);
                    break;
                case DefaultSwitchLabelSyntax _:
                    break;
            }
        }

        private void ParseCasePatternSwitchLabel(CasePatternSwitchLabelSyntax casePatternSwitchLabel)
        {
            ParsePattern(casePatternSwitchLabel.Pattern);
            if (casePatternSwitchLabel.WhenClause != null)
                ParseWhenClause(casePatternSwitchLabel.WhenClause);
        }

        private void ParsePattern(PatternSyntax pattern)
        {
            switch (pattern)
            {
                case BinaryPatternSyntax AsBinaryPattern:
                    ParseBinaryPattern(AsBinaryPattern);
                    break;
                case ConstantPatternSyntax AsConstantPattern:
                    ParseConstantPattern(AsConstantPattern);
                    break;
                case DeclarationPatternSyntax AsDeclarationPattern:
                    ParseDeclarationPattern(AsDeclarationPattern);
                    break;
                case DiscardPatternSyntax _:
                    break;
                case ParenthesizedPatternSyntax AsParenthesizedPattern:
                    ParseParenthesizedPattern(AsParenthesizedPattern);
                    break;
                case RecursivePatternSyntax AsRecursivePattern:
                    ParseRecursivePattern(AsRecursivePattern);
                    break;
                case RelationalPatternSyntax AsRelationalPattern:
                    ParseRelationalPattern(AsRelationalPattern);
                    break;
                case TypePatternSyntax AsTypePattern:
                    ParseTypePattern(AsTypePattern);
                    break;
                case UnaryPatternSyntax AsUnaryPattern:
                    ParseUnaryPattern(AsUnaryPattern);
                    break;
                case VarPatternSyntax AsVarPattern:
                    ParseVarPattern(AsVarPattern);
                    break;
            }
        }

        private void ParseBinaryPattern(BinaryPatternSyntax binaryPattern)
        {
            ParsePattern(binaryPattern.Left);
            ParsePattern(binaryPattern.Right);
        }

        private void ParseConstantPattern(ConstantPatternSyntax constantPattern)
        {
            ParseExpression(constantPattern.Expression);
        }

        private void ParseDeclarationPattern(DeclarationPatternSyntax declarationPattern)
        {
            ParseType(declarationPattern.Type);
            ParseVariableDesignation(declarationPattern.Designation);
        }

        private void ParseParenthesizedPattern(ParenthesizedPatternSyntax parenthesizedPattern)
        {
            ParsePattern(parenthesizedPattern.Pattern);
        }

        private void ParseRecursivePattern(RecursivePatternSyntax recursivePattern)
        {
            if (recursivePattern.Type != null)
                ParseType(recursivePattern.Type);
            if (recursivePattern.PositionalPatternClause != null)
                ParsePositionalPatternClause(recursivePattern.PositionalPatternClause);
            if (recursivePattern.PropertyPatternClause != null)
                ParsePropertyPatternClause(recursivePattern.PropertyPatternClause);
            if (recursivePattern.Designation != null)
                ParseVariableDesignation(recursivePattern.Designation);
        }

        private void ParsePositionalPatternClause(PositionalPatternClauseSyntax positionalPatternClause)
        {
            foreach (SubpatternSyntax Subpattern in positionalPatternClause.Subpatterns)
                ParseSubpattern(Subpattern);
        }

        private void ParsePropertyPatternClause(PropertyPatternClauseSyntax propertyPatternClause)
        {
            foreach (SubpatternSyntax Subpattern in propertyPatternClause.Subpatterns)
                ParseSubpattern(Subpattern);
        }

        private void ParseSubpattern(SubpatternSyntax subpattern)
        {
            if (subpattern.NameColon != null)
                ParseNameColon(subpattern.NameColon);
            ParsePattern(subpattern.Pattern);
        }

        private void ParseRelationalPattern(RelationalPatternSyntax relationalPattern)
        {
            ParseExpression(relationalPattern.Expression);
        }

        private void ParseTypePattern(TypePatternSyntax typePattern)
        {
            ParseType(typePattern.Type);
        }

        private void ParseUnaryPattern(UnaryPatternSyntax unaryPattern)
        {
            ParsePattern(unaryPattern.Pattern);
        }

        private void ParseVarPattern(VarPatternSyntax varPattern)
        {
            ParseVariableDesignation(varPattern.Designation);
        }

        private void ParseWhenClause(WhenClauseSyntax whenClauseSyntax)
        {
            ParseExpression(whenClauseSyntax.Condition);
        }

        private void ParseCaseSwitchLabel(CaseSwitchLabelSyntax caseSwitchLabel)
        {
            ParseExpression(caseSwitchLabel.Value);
        }

        private void ParseThrowStatement(ThrowStatementSyntax throwStatement)
        {
            if (throwStatement.Expression != null)
                ParseExpression(throwStatement.Expression);
        }

        private void ParseTryStatement(TryStatementSyntax tryStatement)
        {
            if (tryStatement.Finally != null)
                ParseFinallyClause(tryStatement.Finally);
            ParseBlock(tryStatement.Block);
            foreach (CatchClauseSyntax CatchClause in tryStatement.Catches)
                ParseCatchClause(CatchClause);
        }

        private void ParseFinallyClause(FinallyClauseSyntax finallyClause)
        {
            ParseBlock(finallyClause.Block);
        }

        private void ParseCatchClause(CatchClauseSyntax catchClause)
        {
            if (catchClause.Declaration != null)
                ParseCatchDeclaration(catchClause.Declaration);
            if (catchClause.Filter != null)
                ParseCatchFilterClause(catchClause.Filter);
            ParseBlock(catchClause.Block);
        }

        private void ParseCatchDeclaration(CatchDeclarationSyntax catchDeclaration)
        {
            ParseType(catchDeclaration.Type);
        }

        private void ParseCatchFilterClause(CatchFilterClauseSyntax catchFilterClause)
        {
            ParseExpression(catchFilterClause.FilterExpression);
        }

        private void ParseUnsafeStatement(UnsafeStatementSyntax unsafeStatement)
        {
            ParseBlock(unsafeStatement.Block);
        }

        private void ParseUsingStatement(UsingStatementSyntax usingStatement)
        {
            ParseStatement(usingStatement.Statement);
            if (usingStatement.Expression != null)
                ParseExpression(usingStatement.Expression);
            if (usingStatement.Declaration != null)
                ParseVariableDeclaration(usingStatement.Declaration, NameCategory.LocalVariable);
        }

        private void ParseWhileStatement(WhileStatementSyntax whileStatement)
        {
            ParseStatement(whileStatement.Statement);
            ParseExpression(whileStatement.Condition);
        }

        private void ParseYieldStatement(YieldStatementSyntax yieldStatement)
        {
            if (yieldStatement.Expression != null)
                ParseExpression(yieldStatement.Expression);
        }

        private void ParseVariableDesignation(VariableDesignationSyntax variableDesignation)
        {
            switch (variableDesignation)
            {

                case DiscardDesignationSyntax _:
                    break;
                case ParenthesizedVariableDesignationSyntax AsParenthesizedVariableDesignation:
                    ParseParenthesizedVariableDesignation(AsParenthesizedVariableDesignation);
                    break;
                case SingleVariableDesignationSyntax AsSingleVariableDesignation:
                    ParseSingleVariableDesignation(AsSingleVariableDesignation);
                    break;
            }
        }

        private void ParseParenthesizedVariableDesignation(ParenthesizedVariableDesignationSyntax parenthesizedVariableDesignation)
        {
            foreach (VariableDesignationSyntax VariableDesignation in parenthesizedVariableDesignation.Variables)
                ParseVariableDesignation(VariableDesignation);
        }

        private void ParseSingleVariableDesignation(SingleVariableDesignationSyntax singleVariableDesignation)
        {
            ParseIdentifier(singleVariableDesignation.Identifier, NameCategory.Neutral);
        }

        private void ParseExpression(ExpressionSyntax expression)
        {
            switch (expression)
            {
                case AnonymousMethodExpressionSyntax AsAnonymousMethodExpression:
                    ParseAnonymousMethodExpression(AsAnonymousMethodExpression);
                    break;
                case ParenthesizedLambdaExpressionSyntax AsParenthesizedLambdaExpression:
                    ParseParenthesizedLambdaExpression(AsParenthesizedLambdaExpression);
                    break;
                case SimpleLambdaExpressionSyntax AsSimpleLambdaExpression:
                    ParseSimpleLambdaExpression(AsSimpleLambdaExpression);
                    break;
                case AnonymousObjectCreationExpressionSyntax AsAnonymousObjectCreationExpression:
                    ParseAnonymousObjectCreationExpression(AsAnonymousObjectCreationExpression);
                    break;
                case ArrayCreationExpressionSyntax AsArrayCreationExpression:
                    ParseArrayCreationExpression(AsArrayCreationExpression);
                    break;
                case AssignmentExpressionSyntax AsAssignmentExpression:
                    ParseAssignmentExpression(AsAssignmentExpression);
                    break;
                case AwaitExpressionSyntax AsAwaitExpression:
                    ParseAwaitExpression(AsAwaitExpression);
                    break;
                case ImplicitObjectCreationExpressionSyntax AsImplicitObjectCreationExpression:
                    ParseImplicitObjectCreationExpression(AsImplicitObjectCreationExpression);
                    break;
                case ObjectCreationExpressionSyntax AsObjectCreationExpression:
                    ParseObjectCreationExpression(AsObjectCreationExpression);
                    break;
                case BinaryExpressionSyntax AsBinaryExpression:
                    ParseBinaryExpression(AsBinaryExpression);
                    break;
                case CastExpressionSyntax AsCastExpression:
                    ParseCastExpression(AsCastExpression);
                    break;
                case CheckedExpressionSyntax AsCheckedExpression:
                    ParseCheckedExpression(AsCheckedExpression);
                    break;
                case ConditionalAccessExpressionSyntax AsConditionalAccessExpression:
                    ParseConditionalAccessExpression(AsConditionalAccessExpression);
                    break;
                case ConditionalExpressionSyntax AsConditionalExpression:
                    ParseConditionalExpression(AsConditionalExpression);
                    break;
                case DeclarationExpressionSyntax AsDeclarationExpression:
                    ParseDeclarationExpression(AsDeclarationExpression);
                    break;
                case DefaultExpressionSyntax AsDefaultExpression:
                    ParseDefaultExpression(AsDefaultExpression);
                    break;
                case ElementAccessExpressionSyntax AsElementAccessExpression:
                    ParseElementAccessExpression(AsElementAccessExpression);
                    break;
                case ElementBindingExpressionSyntax AsElementBindingExpression:
                    ParseElementBindingExpression(AsElementBindingExpression);
                    break;
                case ImplicitArrayCreationExpressionSyntax AsImplicitArrayCreationExpression:
                    ParseImplicitArrayCreationExpression(AsImplicitArrayCreationExpression);
                    break;
                case ImplicitElementAccessSyntax AsImplicitElementAccess:
                    ParseImplicitElementAccess(AsImplicitElementAccess);
                    break;
                case ImplicitStackAllocArrayCreationExpressionSyntax AsImplicitStackAllocArrayCreationExpression:
                    ParseImplicitStackAllocArrayCreationExpression(AsImplicitStackAllocArrayCreationExpression);
                    break;
                case InitializerExpressionSyntax AsInitializerExpression:
                    ParseInitializerExpression(AsInitializerExpression);
                    break;
                case BaseExpressionSyntax _:
                    break;
                case ThisExpressionSyntax _:
                    break;
                case InterpolatedStringExpressionSyntax AsInterpolatedStringExpression:
                    ParseInterpolatedStringExpression(AsInterpolatedStringExpression);
                    break;
                case InvocationExpressionSyntax AsInvocationExpression:
                    ParseInvocationExpression(AsInvocationExpression);
                    break;
                case IsPatternExpressionSyntax AsIsPatternExpression:
                    ParseIsPatternExpression(AsIsPatternExpression);
                    break;
                case LiteralExpressionSyntax _:
                    break;
                case MakeRefExpressionSyntax AsMakeRefExpression:
                    ParseMakeRefExpression(AsMakeRefExpression);
                    break;
                case MemberAccessExpressionSyntax AsMemberAccessExpression:
                    ParseMemberAccessExpression(AsMemberAccessExpression);
                    break;
                case MemberBindingExpressionSyntax AsMemberBindingExpression:
                    ParseMemberBindingExpression(AsMemberBindingExpression);
                    break;
                case OmittedArraySizeExpressionSyntax _:
                    break;
                case ParenthesizedExpressionSyntax AsParenthesizedExpression:
                    ParseParenthesizedExpression(AsParenthesizedExpression);
                    break;
                case PostfixUnaryExpressionSyntax AsPostfixUnaryExpression:
                    ParsePostfixUnaryExpression(AsPostfixUnaryExpression);
                    break;
                case PrefixUnaryExpressionSyntax AsPrefixUnaryExpression:
                    ParsePrefixUnaryExpression(AsPrefixUnaryExpression);
                    break;
                case QueryExpressionSyntax AsQueryExpression:
                    ParseQueryExpression(AsQueryExpression);
                    break;
                case RangeExpressionSyntax AsRangeExpression:
                    ParseRangeExpression(AsRangeExpression);
                    break;
                case RefExpressionSyntax AsRefExpression:
                    ParseRefExpression(AsRefExpression);
                    break;
                case RefTypeExpressionSyntax AsRefTypeExpression:
                    ParseRefTypeExpression(AsRefTypeExpression);
                    break;
                case RefValueExpressionSyntax AsRefValueExpression:
                    ParseRefValueExpression(AsRefValueExpression);
                    break;
                case SizeOfExpressionSyntax AsSizeOfExpression:
                    ParseSizeOfExpression(AsSizeOfExpression);
                    break;
                case StackAllocArrayCreationExpressionSyntax AsStackAllocArrayCreationExpression:
                    ParseStackAllocArrayCreationExpression(AsStackAllocArrayCreationExpression);
                    break;
                case SwitchExpressionSyntax AsSwitchExpression:
                    ParseSwitchExpression(AsSwitchExpression);
                    break;
                case ThrowExpressionSyntax AsThrowExpression:
                    ParseThrowExpression(AsThrowExpression);
                    break;
                case TupleExpressionSyntax AsTupleExpression:
                    ParseTupleExpression(AsTupleExpression);
                    break;
                case TypeOfExpressionSyntax AsTypeOfExpression:
                    ParseTypeOfExpression(AsTypeOfExpression);
                    break;
                case TypeSyntax AsType:
                    ParseType(AsType);
                    break;
                case WithExpressionSyntax AsWithExpression:
                    ParseWithExpression(AsWithExpression);
                    break;
            }
        }

        private void ParseAnonymousMethodExpression(AnonymousMethodExpressionSyntax anonymousMethodExpression)
        {
            ParseBlock(anonymousMethodExpression.Block);
            if (anonymousMethodExpression.ParameterList != null)
                ParseParameterList(anonymousMethodExpression.ParameterList);
            if (anonymousMethodExpression.ExpressionBody != null)
                ParseExpression(anonymousMethodExpression.ExpressionBody);
        }

        private void ParseParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpression)
        {
            if (parenthesizedLambdaExpression.Block != null)
                ParseBlock(parenthesizedLambdaExpression.Block);
            ParseParameterList(parenthesizedLambdaExpression.ParameterList);
            if (parenthesizedLambdaExpression.ExpressionBody != null)
                ParseExpression(parenthesizedLambdaExpression.ExpressionBody);
        }

        private void ParseSimpleLambdaExpression(SimpleLambdaExpressionSyntax simpleLambdaExpression)
        {
            if (simpleLambdaExpression.Block != null)
                ParseBlock(simpleLambdaExpression.Block);
            ParseParameter(simpleLambdaExpression.Parameter);
            if (simpleLambdaExpression.ExpressionBody != null)
                ParseExpression(simpleLambdaExpression.ExpressionBody);
        }

        private void ParseAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax anonymousObjectCreationExpression)
        {
            foreach (AnonymousObjectMemberDeclaratorSyntax AnonymousObjectMemberDeclarator in anonymousObjectCreationExpression.Initializers)
                ParseAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclarator);
        }

        private void ParseAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax anonymousObjectMemberDeclarator)
        {
            if (anonymousObjectMemberDeclarator.NameEquals != null)
                ParseNameEquals(anonymousObjectMemberDeclarator.NameEquals);
            ParseExpression(anonymousObjectMemberDeclarator.Expression);
        }

        private void ParseNameEquals(NameEqualsSyntax nameEquals)
        {
            ParseIdentifierName(nameEquals.Name, NameCategory.LocalVariable);
        }

        private void ParseArrayCreationExpression(ArrayCreationExpressionSyntax arrayCreationExpression)
        {
            ParseArrayType(arrayCreationExpression.Type);
            if (arrayCreationExpression.Initializer != null)
                ParseInitializerExpression(arrayCreationExpression.Initializer);
        }

        private void ParseAssignmentExpression(AssignmentExpressionSyntax assignmentExpression)
        {
            ParseExpression(assignmentExpression.Left);
            ParseExpression(assignmentExpression.Right);
        }

        private void ParseAwaitExpression(AwaitExpressionSyntax awaitExpression)
        {
            ParseExpression(awaitExpression.Expression);
        }

        private void ParseImplicitObjectCreationExpression(ImplicitObjectCreationExpressionSyntax implicitObjectCreationExpression)
        {
            ParseArgumentList(implicitObjectCreationExpression.ArgumentList);
            if (implicitObjectCreationExpression.Initializer != null)
                ParseInitializerExpression(implicitObjectCreationExpression.Initializer);
        }

        private void ParseObjectCreationExpression(ObjectCreationExpressionSyntax objectCreationExpression)
        {
            ParseType(objectCreationExpression.Type);
            if (objectCreationExpression.ArgumentList != null)
                ParseArgumentList(objectCreationExpression.ArgumentList);
            if (objectCreationExpression.Initializer != null)
                ParseInitializerExpression(objectCreationExpression.Initializer);
        }

        private void ParseBinaryExpression(BinaryExpressionSyntax binaryExpression)
        {
            ParseExpression(binaryExpression.Left);
            ParseExpression(binaryExpression.Right);
        }

        private void ParseCastExpression(CastExpressionSyntax castExpression)
        {
            ParseType(castExpression.Type);
            ParseExpression(castExpression.Expression);
        }

        private void ParseCheckedExpression(CheckedExpressionSyntax checkedExpression)
        {
            ParseExpression(checkedExpression.Expression);
        }

        private void ParseConditionalAccessExpression(ConditionalAccessExpressionSyntax conditionalAccessExpression)
        {
            ParseExpression(conditionalAccessExpression.Expression);
            ParseExpression(conditionalAccessExpression.WhenNotNull);
        }

        private void ParseConditionalExpression(ConditionalExpressionSyntax conditionalExpression)
        {
            ParseExpression(conditionalExpression.Condition);
            ParseExpression(conditionalExpression.WhenTrue);
            ParseExpression(conditionalExpression.WhenFalse);
        }

        private void ParseDeclarationExpression(DeclarationExpressionSyntax declarationExpression)
        {
            ParseType(declarationExpression.Type);
            ParseVariableDesignation(declarationExpression.Designation);
        }

        private void ParseDefaultExpression(DefaultExpressionSyntax defaultExpression)
        {
            ParseType(defaultExpression.Type);
        }

        private void ParseElementAccessExpression(ElementAccessExpressionSyntax elementAccessExpression)
        {
            ParseExpression(elementAccessExpression.Expression);
            ParseBracketedArgumentList(elementAccessExpression.ArgumentList);
        }

        private void ParseElementBindingExpression(ElementBindingExpressionSyntax elementBindingExpression)
        {
            ParseBracketedArgumentList(elementBindingExpression.ArgumentList);
        }

        private void ParseImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax implicitArrayCreationExpression)
        {
            ParseInitializerExpression(implicitArrayCreationExpression.Initializer);
        }

        private void ParseImplicitElementAccess(ImplicitElementAccessSyntax implicitElementAccess)
        {
            ParseBracketedArgumentList(implicitElementAccess.ArgumentList);
        }

        private void ParseImplicitStackAllocArrayCreationExpression(ImplicitStackAllocArrayCreationExpressionSyntax implicitStackAllocArrayCreationExpression)
        {
            ParseInitializerExpression(implicitStackAllocArrayCreationExpression.Initializer);
        }

        private void ParseInitializerExpression(InitializerExpressionSyntax initializerExpression)
        {
            foreach (ExpressionSyntax Expression in initializerExpression.Expressions)
                ParseExpression(Expression);
        }

        private void ParseInterpolatedStringExpression(InterpolatedStringExpressionSyntax interpolatedStringExpression)
        {
            foreach (InterpolatedStringContentSyntax InterpolatedStringContent in interpolatedStringExpression.Contents)
                ParseInterpolatedStringContent(InterpolatedStringContent);
        }

        private void ParseInterpolatedStringContent(InterpolatedStringContentSyntax interpolatedStringContent)
        {
            switch (interpolatedStringContent)
            {
                case InterpolatedStringTextSyntax _:
                    break;
                case InterpolationSyntax AsInterpolation:
                    ParseInterpolation(AsInterpolation);
                    break;
            }
        }

        private void ParseInterpolation(InterpolationSyntax interpolation)
        {
            ParseExpression(interpolation.Expression);
            if (interpolation.AlignmentClause != null)
                ParseInterpolationAlignmentClause(interpolation.AlignmentClause);
            if (interpolation.FormatClause != null)
                ParseInterpolationFormatClause(interpolation.FormatClause);
        }

        private void ParseInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax interpolationAlignmentClause)
        {
            ParseExpression(interpolationAlignmentClause.Value);
        }

        private void ParseInterpolationFormatClause(InterpolationFormatClauseSyntax _)
        {
        }

        private void ParseInvocationExpression(InvocationExpressionSyntax invocationExpression)
        {
            ParseExpression(invocationExpression.Expression);
            ParseArgumentList(invocationExpression.ArgumentList);
        }

        private void ParseIsPatternExpression(IsPatternExpressionSyntax isPatternExpression)
        {
            ParseExpression(isPatternExpression.Expression);
            ParsePattern(isPatternExpression.Pattern);
        }

        private void ParseMakeRefExpression(MakeRefExpressionSyntax makeRefExpression)
        {
            ParseExpression(makeRefExpression.Expression);
        }

        private void ParseMemberAccessExpression(MemberAccessExpressionSyntax memberAccessExpression)
        {
            ParseExpression(memberAccessExpression.Expression);
            ParseSimpleName(memberAccessExpression.Name, NameCategory.Neutral);
        }

        private void ParseMemberBindingExpression(MemberBindingExpressionSyntax memberBindingExpression)
        {
            ParseSimpleName(memberBindingExpression.Name, NameCategory.Neutral);
        }

        private void ParseParenthesizedExpression(ParenthesizedExpressionSyntax parenthesizedExpression)
        {
            ParseExpression(parenthesizedExpression.Expression);
        }

        private void ParsePostfixUnaryExpression(PostfixUnaryExpressionSyntax postfixUnaryExpression)
        {
            ParseExpression(postfixUnaryExpression.Operand);
        }

        private void ParsePrefixUnaryExpression(PrefixUnaryExpressionSyntax prefixUnaryExpression)
        {
            ParseExpression(prefixUnaryExpression.Operand);
        }

        private void ParseQueryExpression(QueryExpressionSyntax queryExpression)
        {
            ParseFromClause(queryExpression.FromClause);
            ParseQueryBody(queryExpression.Body);
        }

        private void ParseQueryBody(QueryBodySyntax queryBody)
        {
            foreach (QueryClauseSyntax QueryClause in queryBody.Clauses)
                ParseQueryClause(QueryClause);
            ParseSelectOrGroupClause(queryBody.SelectOrGroup);
            if (queryBody.Continuation != null)
                ParseQueryContinuation(queryBody.Continuation);
        }

        private void ParseQueryClause(QueryClauseSyntax queryClause)
        {
            switch (queryClause)
            {
                case FromClauseSyntax AsFromClause:
                    ParseFromClause(AsFromClause);
                    break;
                case JoinClauseSyntax AsJoinClause:
                    ParseJoinClause(AsJoinClause);
                    break;
                case LetClauseSyntax AsLetClause:
                    ParseLetClause(AsLetClause);
                    break;
                case OrderByClauseSyntax AsOrderByClause:
                    ParseOrderByClause(AsOrderByClause);
                    break;
                case WhereClauseSyntax AsWhereClause:
                    ParseWhereClause(AsWhereClause);
                    break;
            }
        }

        private void ParseFromClause(FromClauseSyntax fromClause)
        {
            if (fromClause.Type != null)
                ParseType(fromClause.Type);
            ParseExpression(fromClause.Expression);
        }

        private void ParseJoinClause(JoinClauseSyntax joinClause)
        {
            if (joinClause.Into != null)
                ParseJoinIntoClause(joinClause.Into);
            ParseExpression(joinClause.LeftExpression);
            ParseExpression(joinClause.InExpression);
            ParseIdentifier(joinClause.Identifier, NameCategory.LocalVariable);
            if (joinClause.Type != null)
                ParseType(joinClause.Type);
            ParseExpression(joinClause.RightExpression);
        }

        private void ParseJoinIntoClause(JoinIntoClauseSyntax joinIntoClause)
        {
            ParseIdentifier(joinIntoClause.Identifier, NameCategory.LocalVariable);
        }

        private void ParseLetClause(LetClauseSyntax letClause)
        {
            ParseIdentifier(letClause.Identifier, NameCategory.LocalVariable);
            ParseExpression(letClause.Expression);
        }

        private void ParseOrderByClause(OrderByClauseSyntax orderByClause)
        {
            foreach (OrderingSyntax Ordering in orderByClause.Orderings)
                ParseOrdering(Ordering);
        }

        private void ParseOrdering(OrderingSyntax ordering)
        {
            ParseExpression(ordering.Expression);
        }

        private void ParseWhereClause(WhereClauseSyntax whereClause)
        {
            ParseExpression(whereClause.Condition);
        }

        private void ParseSelectOrGroupClause(SelectOrGroupClauseSyntax selectOrGroupClause)
        {
            switch (selectOrGroupClause)
            {
                case GroupClauseSyntax AsGroupClause:
                    ParseGroupClause(AsGroupClause);
                    break;
                case SelectClauseSyntax AsSelectClause:
                    ParseSelectClause(AsSelectClause);
                    break;
            }
        }

        private void ParseGroupClause(GroupClauseSyntax groupClause)
        {
            ParseExpression(groupClause.GroupExpression);
            ParseExpression(groupClause.ByExpression);
        }

        private void ParseSelectClause(SelectClauseSyntax selectClause)
        {
            ParseExpression(selectClause.Expression);
        }

        private void ParseQueryContinuation(QueryContinuationSyntax queryContinuation)
        {
            ParseIdentifier(queryContinuation.Identifier, NameCategory.LocalVariable);
            ParseQueryBody(queryContinuation.Body);
        }

        private void ParseRangeExpression(RangeExpressionSyntax rangeExpression)
        {
            if (rangeExpression.LeftOperand != null)
                ParseExpression(rangeExpression.LeftOperand);
            if (rangeExpression.RightOperand != null)
                ParseExpression(rangeExpression.RightOperand);
        }

        private void ParseRefExpression(RefExpressionSyntax refExpression)
        {
            ParseExpression(refExpression.Expression);
        }

        private void ParseRefTypeExpression(RefTypeExpressionSyntax refTypeExpression)
        {
            ParseExpression(refTypeExpression.Expression);
        }

        private void ParseRefValueExpression(RefValueExpressionSyntax refValueExpression)
        {
            ParseExpression(refValueExpression.Expression);
            ParseType(refValueExpression.Type);
        }

        private void ParseSizeOfExpression(SizeOfExpressionSyntax sizeOfExpression)
        {
            ParseType(sizeOfExpression.Type);
        }

        private void ParseStackAllocArrayCreationExpression(StackAllocArrayCreationExpressionSyntax stackAllocArrayCreationExpression)
        {
            ParseType(stackAllocArrayCreationExpression.Type);
            if (stackAllocArrayCreationExpression.Initializer != null)
                ParseInitializerExpression(stackAllocArrayCreationExpression.Initializer);
        }

        private void ParseSwitchExpression(SwitchExpressionSyntax switchExpression)
        {
            ParseExpression(switchExpression.GoverningExpression);
            foreach (SwitchExpressionArmSyntax SwitchExpressionArm in switchExpression.Arms)
                ParseSwitchExpressionArm(SwitchExpressionArm);
        }

        private void ParseSwitchExpressionArm(SwitchExpressionArmSyntax switchExpressionArm)
        {
            ParsePattern(switchExpressionArm.Pattern);
            if (switchExpressionArm.WhenClause != null)
                ParseWhenClause(switchExpressionArm.WhenClause);
            ParseExpression(switchExpressionArm.Expression);
        }

        private void ParseThrowExpression(ThrowExpressionSyntax throwExpression)
        {
            ParseExpression(throwExpression.Expression);
        }

        private void ParseTupleExpression(TupleExpressionSyntax tupleExpression)
        {
            foreach (ArgumentSyntax Argument in tupleExpression.Arguments)
                ParseArgument(Argument);
        }

        private void ParseTypeOfExpression(TypeOfExpressionSyntax typeOfExpression)
        {
            ParseType(typeOfExpression.Type);
        }

        private void ParseWithExpression(WithExpressionSyntax withExpression)
        {
            ParseExpression(withExpression.Expression);
            ParseInitializerExpression(withExpression.Initializer);
        }
        #endregion
    }
}
