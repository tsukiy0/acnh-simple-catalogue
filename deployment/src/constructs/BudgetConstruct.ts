import { Construct } from "@aws-cdk/core";
import { CfnBudget } from "@aws-cdk/aws-budgets";

export class BudgetConstruct extends Construct {
  constructor(
    scope: Construct,
    id: string,
    props: {
      amount: number;
      email: string;
    }
  ) {
    super(scope, id);

    new CfnBudget(this, "Budget", {
      budget: {
        budgetType: "COST",
        timeUnit: "MONTHLY",
        budgetLimit: {
          amount: props.amount,
          unit: "USD",
        },
      },
      notificationsWithSubscribers: [100].map((threshold) => ({
        notification: {
          notificationType: "ACTUAL",
          thresholdType: "PERCENTAGE",
          comparisonOperator: "GREATER_THAN",
          threshold,
        },
        subscribers: [
          {
            subscriptionType: "EMAIL",
            address: props.email,
          },
        ],
      })),
    });
  }
}
