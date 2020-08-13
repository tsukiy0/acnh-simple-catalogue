import { Construct, CfnOutput } from "@aws-cdk/core";

export class OutputConstruct extends Construct {
  constructor(
    scope: Construct,
    id: string,
    props: {
      value: string;
    }
  ) {
    super(scope, id);

    new CfnOutput(this, "Output", {
      value: props.value,
    }).overrideLogicalId(id);
  }
}
