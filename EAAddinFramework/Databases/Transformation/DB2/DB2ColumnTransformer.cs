﻿using System;
using System.Collections.Generic;
using System.Linq;
using UML=TSF.UmlToolingFramework.UML;
using UTF_EA=TSF.UmlToolingFramework.Wrappers.EA;
using DB=DatabaseFramework;
using DB_EA = EAAddinFramework.Databases;
using EAAddinFramework.Utilities;

namespace EAAddinFramework.Databases.Transformation.DB2
{
	/// <summary>
	/// Description of DB2ColumnTransformer.
	/// </summary>
	public class DB2ColumnTransformer:EAColumnTransformer
	{
		public DB2ColumnTransformer(Table table):base(table){}

		#region implemented abstract members of EAColumnTransformer

		public override DB.Column transformLogicalProperty(UML.Classes.Kernel.Property property)
		{
			//TODO: translate name to alias
			this.logicalProperty = property;
			if (property is UTF_EA.Attribute) return transformLogicalAttribute((UTF_EA.Attribute)property);
			if (property is UTF_EA.AssociationEnd) return transformLogicalAssociationEnd((UTF_EA.AssociationEnd)property);
			//if neither then we have something weird here.
			return null;
		}
		private Column transformLogicalAttribute(UTF_EA.Attribute attribute)
		{
			this.column = new Column(this._table, attribute.alias);
			//get base type
			var attributeType = attribute.type as UTF_EA.ElementWrapper;
			if (attributeType == null) Logger.logError (string.Format("Attribute {0}.{1} does not have a element as datatype"
			                                                    ,attribute.owner.name, attribute.name));
			else
			{
				DataType datatype = _table._owner._factory.createDataType(attributeType.alias);
				if (datatype == null) Logger.logError (string.Format("Could not find translate {0} as Datatype for attribute {1}.{2}"
				                                                    ,attributeType.alias, attribute.owner.name, attribute.name));
				else
				{
					column.type = datatype;
				}
			}
			//set not null property
			if (attribute.lower == 0)
			{
				column.isNotNullable = false;
			}
			else
			{
				column.isNotNullable = true;
			}
			return this._column;
		}
		private Column transformLogicalAssociationEnd(UTF_EA.AssociationEnd associationEnd)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
