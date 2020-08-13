import { Construct, Stack, StackProps } from "@aws-cdk/core";
import { BudgetConstruct } from "./constructs/BudgetConstruct";
import { WebConstruct } from "./constructs/WebConstruct";

export class RootStack extends Stack {
  constructor(scope: Construct, id: string, props: StackProps) {
    super(scope, id, props);

    const email = process.env.NOTIFICATION_EMAIL as string;

    new BudgetConstruct(this, "Budget", {
      amount: 20,
      email,
    });

    new WebConstruct(this, "Web");
  }
}
